// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    ///     A representation of a type
    /// </summary>
    public class DynamicType {
        private const BindingFlags AllBindingFlags = BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public |
            BindingFlags.NonPublic;


        private readonly ConcurrentDictionary<MemberInfo, object> _memberMap = new();

        private DynamicCtor[] _constructors;


        private Optional<DynamicCtor> _defaultConstructor;
        private DynamicField[] _fields;
        private DynamicInvoke[] _methods;
        private DynamicProperty[] _properties;


        /// <summary>
        ///     The original type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///     The name
        /// </summary>
        public string Name { get; }

        public DynamicCtor DefaultConstructor => _defaultConstructor.IsDefined
            ? _defaultConstructor.Value
            : (_defaultConstructor = CreateDefaultConstructor()).Value;


        /// <summary>
        ///     The constructors
        /// </summary>
        public IEnumerable<DynamicCtor> Constructors => _constructors ?? (_constructors = CreateConstructors());

        /// <summary>
        ///     The methods
        /// </summary>
        public IEnumerable<DynamicInvoke> Methods => _methods ?? (_methods = CreateMethods());

        /// <summary>
        ///     The accessors
        /// </summary>
        public IEnumerable<DynamicAccessor> Accessors => Properties.Concat<DynamicAccessor>(Fields);

        /// <summary>
        ///     The properties
        /// </summary>
        public IEnumerable<DynamicProperty> Properties => _properties ?? (_properties = CreateProperties());

        /// <summary>
        ///     The fields
        /// </summary>
        public IEnumerable<DynamicField> Fields => _fields ?? (_fields = CreateFields());

        /// <summary>
        ///     Checks if there is a default constructor
        /// </summary>
        public bool HasDefaultConstructor => DefaultConstructor is not null;

        /// <summary>
        ///     Creates a new instance of <see cref="DynamicType" />
        /// </summary>
        public DynamicType(Type type) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = type.Name;
        }

        #region Object
        /// <summary>
        ///     Returns the name of the type
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Name;
        }
        #endregion

        #region Constructors
        /// <summary>
        ///     When overridden in a derived class, searches for the constructors defined for the current
        ///     <see cref="T:System.Type"></see>, using the specified BindingFlags.
        /// </summary>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>
        ///     An array of <see cref="T:System.Reflection.ConstructorInfo"></see> objects representing all constructors
        ///     defined for the current <see cref="T:System.Type"></see> that match the specified binding constraints, including
        ///     the type initializer if it is defined. Returns an empty array of type
        ///     <see cref="T:System.Reflection.ConstructorInfo"></see> if no constructors are defined for the current
        ///     <see cref="T:System.Type"></see>, if none of the defined constructors match the binding constraints, or if the
        ///     current <see cref="T:System.Type"></see> represents a type parameter in the definition of a generic type or generic
        ///     method.
        /// </returns>
        public IEnumerable<DynamicCtor> GetConstructors(BindingFlags bindingAttr) {
            var ctors = Type.GetConstructors(bindingAttr);
            return ctors.Select(RegisterConstructor)
               .SkipNull();
        }

        /// <summary>
        ///     Searches for a constructor whose parameters match the specified argument types and modifiers, using the
        ///     specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <param name="binder">
        ///     An object that defines a set of properties and enables binding, which can involve selection of an
        ///     overloaded method, coercion of argument types, and invocation of a member through reflection.   -or-   A null
        ///     reference (Nothing in Visual Basic), to use the <see cref="P:System.Type.DefaultBinder"></see>.
        /// </param>
        /// <param name="callConvention">
        ///     The object that specifies the set of rules to use regarding the order and layout of
        ///     arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.
        /// </param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the constructor to get.   -or-   An empty array of the type <see cref="T:System.Type"></see>
        ///     (that is, Type[] types = new Type[0]) to get a constructor that takes no parameters.
        /// </param>
        /// <param name="modifiers">
        ///     An array of <see cref="T:System.Reflection.ParameterModifier"></see> objects representing the
        ///     attributes associated with the corresponding element in the types array. The default binder does not process this
        ///     parameter.
        /// </param>
        /// <returns>An object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="types">types</paramref> is null.   -or-   One of the
        ///     elements in <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="types">types</paramref> is multidimensional.   -or-
        ///     <paramref name="modifiers">modifiers</paramref> is multidimensional.   -or-
        ///     <paramref name="types">types</paramref> and <paramref name="modifiers">modifiers</paramref> do not have the same
        ///     length.
        /// </exception>
        public DynamicCtor GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) {
            var ctor = Type.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
            return ctor is not null
                ? RegisterConstructor(ctor)
                : null;
        }

        /// <summary>
        ///     Searches for a constructor whose parameters match the specified argument types and modifiers, using the
        ///     specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <param name="binder">
        ///     An object that defines a set of properties and enables binding, which can involve selection of an
        ///     overloaded method, coercion of argument types, and invocation of a member through reflection.   -or-   A null
        ///     reference (Nothing in Visual Basic), to use the <see cref="P:System.Type.DefaultBinder"></see>.
        /// </param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the constructor to get.   -or-   An empty array of the type <see cref="T:System.Type"></see>
        ///     (that is, Type[] types = new Type[0]) to get a constructor that takes no parameters.   -or-
        ///     <see cref="F:System.Type.EmptyTypes"></see>.
        /// </param>
        /// <param name="modifiers">
        ///     An array of <see cref="T:System.Reflection.ParameterModifier"></see> objects representing the
        ///     attributes associated with the corresponding element in the parameter type array. The default binder does not
        ///     process this parameter.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Reflection.ConstructorInfo"></see> object representing the constructor that matches the
        ///     specified requirements, if found; otherwise, null.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="types">types</paramref> is null.   -or-   One of the
        ///     elements in <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="types">types</paramref> is multidimensional.   -or-
        ///     <paramref name="modifiers">modifiers</paramref> is multidimensional.   -or-
        ///     <paramref name="types">types</paramref> and <paramref name="modifiers">modifiers</paramref> do not have the same
        ///     length.
        /// </exception>
        public DynamicCtor GetConstructor(BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) {
            var ctor = Type.GetConstructor(bindingAttr, binder, types, modifiers);
            return ctor is not null
                ? RegisterConstructor(ctor)
                : null;
        }

        /// <summary>Searches for a public instance constructor whose parameters match the types in the specified array.</summary>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the desired constructor.   -or-   An empty array of <see cref="T:System.Type"></see> objects, to
        ///     get a constructor that takes no parameters. Such an empty array is provided by the static field
        ///     <see cref="F:System.Type.EmptyTypes"></see>.
        /// </param>
        /// <returns>
        ///     An object representing the instance constructor whose parameters match the types in the parameter type
        ///     array, if found; otherwise, null.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="types">types</paramref> is null.   -or-   One of the
        ///     elements in <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="types">types</paramref> is multidimensional.</exception>
        public DynamicCtor GetConstructor(params Type[] types) {
            var ctor = Type.GetConstructor(types);
            return ctor is not null
                ? RegisterConstructor(ctor)
                : null;
        }
        #endregion

        #region Fields
        /// <summary>
        ///     Searches for the fields defined for the current
        /// </summary>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>
        ///     An array of <see cref="T:System.Reflection.FieldInfo"></see> objects representing all fields defined for the
        ///     current <see cref="T:System.Type"></see> that match the specified binding constraints.   -or-   An empty array of
        ///     type <see cref="T:System.Reflection.FieldInfo"></see>, if no fields are defined for the current
        ///     <see cref="T:System.Type"></see>, or if none of the defined fields match the binding constraints.
        /// </returns>
        public IEnumerable<DynamicField> GetFields(BindingFlags bindingAttr) {
            var fields = Type.GetFields(bindingAttr);
            return fields.Select(RegisterField)
               .SkipNull();
        }

        /// <summary>
        /// Searches for the field with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the field with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     This <see cref="T:System.Type"></see> object is a
        ///     <see cref="T:System.Reflection.Emit.TypeBuilder"></see> whose
        ///     <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType"></see> method has not yet been called.
        /// </exception>
        public DynamicField GetField(string name) {
            var field = Type.GetField(name);
            return field is not null
                ? RegisterField(field)
                : null;
        }

        /// <summary>Searches for the specified field, using the specified binding constraints.</summary>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>An object representing the field that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        public DynamicField GetField(string name, BindingFlags bindingAttr) {
            var field = Type.GetField(name, bindingAttr);
            return field is not null
                ? RegisterField(field)
                : null;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Searches for the methods defined for the current
        ///     <see cref="T:System.Type"></see>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>
        ///     An array of <see cref="T:System.Reflection.MethodInfo"></see> objects representing all methods defined for the
        ///     current <see cref="T:System.Type"></see> that match the specified binding constraints.   -or-   An empty array of
        ///     type <see cref="T:System.Reflection.MethodInfo"></see>, if no methods are defined for the current
        ///     <see cref="T:System.Type"></see>, or if none of the defined methods match the binding constraints.
        /// </returns>
        public IEnumerable<DynamicInvoke> GetMethods(BindingFlags bindingAttr) {
            var methods = Type.GetMethods(bindingAttr);
            return methods.Select(RegisterMethod)
               .SkipNull();
        }

        /// <summary>Searches for the method with the specified name.</summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <returns>An object that represents the method with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method is found with the specified name.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        public DynamicInvoke GetMethod(string name) {
            var method = Type.GetMethod(name);
            return method is not null
                ? RegisterMethod(method)
                : null;
        }

        /// <summary>Searches for the specified method, using the specified binding constraints.</summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one method is found with the specified name and
        ///     matching the specified binding constraints.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        public DynamicInvoke GetMethod(string name, BindingFlags bindingAttr) {
            var method = Type.GetMethod(name, bindingAttr);
            return method is not null
                ? RegisterMethod(method)
                : null;
        }

        /// <summary>
        ///     Searches for the specified method whose parameters match the specified argument types and modifiers, using the
        ///     specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <param name="binder">
        ///     An object that defines a set of properties and enables binding, which can involve selection of an
        ///     overloaded method, coercion of argument types, and invocation of a member through reflection.   -or-   A null
        ///     reference (Nothing in Visual Basic), to use the <see cref="P:System.Type.DefaultBinder"></see>.
        /// </param>
        /// <param name="callConvention">
        ///     The object that specifies the set of rules to use regarding the order and layout of
        ///     arguments, how the return value is passed, what registers are used for arguments, and how the stack is cleaned up.
        /// </param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the method to get.   -or-   An empty array of <see cref="T:System.Type"></see> objects (as
        ///     provided by the <see cref="F:System.Type.EmptyTypes"></see> field) to get a method that takes no parameters.
        /// </param>
        /// <param name="modifiers">
        ///     An array of <see cref="T:System.Reflection.ParameterModifier"></see> objects representing the
        ///     attributes associated with the corresponding element in the types array. To be only used when calling through COM
        ///     interop, and only parameters that are passed by reference are handled. The default binder does not process this
        ///     parameter.
        /// </param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one method is found with the specified name and
        ///     matching the specified binding constraints.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.   -or-   One of the elements in
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="types">types</paramref> is multidimensional.   -or-
        ///     <paramref name="modifiers">modifiers</paramref> is multidimensional.
        /// </exception>
        public DynamicInvoke GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) {
            var method = Type.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
            return method is not null
                ? RegisterMethod(method)
                : null;
        }

        /// <summary>
        ///     Searches for the specified method whose parameters match the specified argument types and modifiers, using the
        ///     specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <param name="binder">
        ///     An object that defines a set of properties and enables binding, which can involve selection of an
        ///     overloaded method, coercion of argument types, and invocation of a member through reflection.   -or-   A null
        ///     reference (Nothing in Visual Basic), to use the <see cref="P:System.Type.DefaultBinder"></see>.
        /// </param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the method to get.   -or-   An empty array of <see cref="T:System.Type"></see> objects (as
        ///     provided by the <see cref="F:System.Type.EmptyTypes"></see> field) to get a method that takes no parameters.
        /// </param>
        /// <param name="modifiers">
        ///     An array of <see cref="T:System.Reflection.ParameterModifier"></see> objects representing the
        ///     attributes associated with the corresponding element in the types array. To be only used when calling through COM
        ///     interop, and only parameters that are passed by reference are handled. The default binder does not process this
        ///     parameter.
        /// </param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one method is found with the specified name and
        ///     matching the specified binding constraints.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.   -or-   One of the elements in
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="types">types</paramref> is multidimensional.   -or-
        ///     <paramref name="modifiers">modifiers</paramref> is multidimensional.
        /// </exception>
        public DynamicInvoke GetMethod(
            string name,
            BindingFlags bindingAttr,
            Binder binder,
            Type[] types,
            ParameterModifier[] modifiers) {
            var method = Type.GetMethod(name, bindingAttr, binder, types, modifiers);
            return method is not null
                ? RegisterMethod(method)
                : null;
        }

        /// <summary>Searches for the specified public method whose parameters match the specified argument types.</summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the method to get.   -or-   An empty array of <see cref="T:System.Type"></see> objects (as
        ///     provided by the <see cref="F:System.Type.EmptyTypes"></see> field) to get a method that takes no parameters.
        /// </param>
        /// <returns>
        ///     An object representing the method whose parameters match the specified argument types, if found;
        ///     otherwise, null.
        /// </returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one method is found with the specified name and
        ///     specified parameters.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.   -or-   One of the elements in
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="types">types</paramref> is multidimensional.</exception>
        public DynamicInvoke GetMethod(string name, params Type[] types) {
            var method = Type.GetMethod(name, types);
            return method is not null
                ? RegisterMethod(method)
                : null;
        }

        /// <summary>Searches for the specified public method whose parameters match the specified argument types and modifiers.</summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the method to get.   -or-   An empty array of <see cref="T:System.Type"></see> objects (as
        ///     provided by the <see cref="F:System.Type.EmptyTypes"></see> field) to get a method that takes no parameters.
        /// </param>
        /// <param name="modifiers">
        ///     An array of <see cref="T:System.Reflection.ParameterModifier"></see> objects representing the
        ///     attributes associated with the corresponding element in the types array. To be only used when calling through COM
        ///     interop, and only parameters that are passed by reference are handled. The default binder does not process this
        ///     parameter.
        /// </param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one method is found with the specified name and
        ///     specified parameters.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.   -or-   One of the elements in
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="types">types</paramref> is multidimensional.   -or-
        ///     <paramref name="modifiers">modifiers</paramref> is multidimensional.
        /// </exception>
        public DynamicInvoke GetMethod(string name, Type[] types, ParameterModifier[] modifiers) {
            var method = Type.GetMethod(name, types, modifiers);
            return method is not null
                ? RegisterMethod(method)
                : null;
        }
        #endregion

        #region Properties
        /// <summary>
        ///     When overridden in a derived class, searches for the properties of the current
        ///     <see cref="T:System.Type"></see>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>
        ///     An array of <see cref="T:System.Reflection.PropertyInfo"></see> objects representing all properties of the
        ///     current <see cref="T:System.Type"></see> that match the specified binding constraints.   -or-   An empty array of
        ///     type <see cref="T:System.Reflection.PropertyInfo"></see>, if the current <see cref="T:System.Type"></see> does not
        ///     have properties, or if none of the properties match the binding constraints.
        /// </returns>
        public IEnumerable<DynamicProperty> GetProperties(BindingFlags bindingAttr) {
            var properties = Type.GetProperties(bindingAttr);
            return properties.Select(RegisterProperty)
               .SkipNull();
        }

        /// <summary>Searches for the property with the specified name.</summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <returns>An object representing the property with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one property is found with the specified name.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        public DynamicProperty GetProperty(string name) {
            var property = Type.GetProperty(name);
            return property is not null
                ? RegisterProperty(property)
                : null;
        }

        /// <summary>Searches for the specified property, using the specified binding constraints.</summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one property is found with the specified name
        ///     and matching the specified binding constraints.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        public DynamicProperty GetProperty(string name, BindingFlags bindingAttr) {
            var property = Type.GetProperty(name, bindingAttr);
            return property is not null
                ? RegisterProperty(property)
                : null;
        }

        /// <summary>
        ///     Searches for the specified property whose parameters match the specified argument types and modifiers, using
        ///     the specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <param name="binder">
        ///     An object that defines a set of properties and enables binding, which can involve selection of an
        ///     overloaded method, coercion of argument types, and invocation of a member through reflection.   -or-   A null
        ///     reference (Nothing in Visual Basic), to use the <see cref="P:System.Type.DefaultBinder"></see>.
        /// </param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the indexed property to get.   -or-   An empty array of the type
        ///     <see cref="T:System.Type"></see> (that is, Type[] types = new Type[0]) to get a property that is not indexed.
        /// </param>
        /// <param name="modifiers">
        ///     An array of <see cref="T:System.Reflection.ParameterModifier"></see> objects representing the
        ///     attributes associated with the corresponding element in the types array. The default binder does not process this
        ///     parameter.
        /// </param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one property is found with the specified name
        ///     and matching the specified binding constraints.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="types">types</paramref> is multidimensional.   -or-
        ///     <paramref name="modifiers">modifiers</paramref> is multidimensional.   -or-
        ///     <paramref name="types">types</paramref> and <paramref name="modifiers">modifiers</paramref> do not have the same
        ///     length.
        /// </exception>
        /// <exception cref="T:System.NullReferenceException">An element of <paramref name="types">types</paramref> is null.</exception>
        public DynamicProperty GetProperty(
            string name,
            BindingFlags bindingAttr,
            Binder binder,
            Type returnType,
            Type[] types,
            ParameterModifier[] modifiers) {
            var property = Type.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
            return property is not null
                ? RegisterProperty(property)
                : null;
        }

        /// <summary>Searches for the property with the specified name and return type.</summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <returns>An object representing the property with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one property is found with the specified name.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null, or
        ///     <paramref name="returnType">returnType</paramref> is null.
        /// </exception>
        public DynamicProperty GetProperty(string name, Type returnType) {
            var property = Type.GetProperty(name, returnType);
            return property is not null
                ? RegisterProperty(property)
                : null;
        }

        /// <summary>Searches for the specified public property whose parameters match the specified argument types.</summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the indexed property to get.   -or-   An empty array of the type
        ///     <see cref="T:System.Type"></see> (that is, Type[] types = new Type[0]) to get a property that is not indexed.
        /// </param>
        /// <returns>
        ///     An object representing the property whose parameters match the specified argument types, if found;
        ///     otherwise, null.
        /// </returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one property is found with the specified name
        ///     and matching the specified argument types.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="types">types</paramref> is multidimensional.</exception>
        /// <exception cref="T:System.NullReferenceException">An element of <paramref name="types">types</paramref> is null.</exception>
        public DynamicProperty GetProperty(string name, Type returnType, Type[] types) {
            var property = Type.GetProperty(name, returnType, types);
            return property is not null
                ? RegisterProperty(property)
                : null;
        }

        /// <summary>Searches for the specified public property whose parameters match the specified argument types and modifiers.</summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the indexed property to get.   -or-   An empty array of the type
        ///     <see cref="T:System.Type"></see> (that is, Type[] types = new Type[0]) to get a property that is not indexed.
        /// </param>
        /// <param name="modifiers">
        ///     An array of <see cref="T:System.Reflection.ParameterModifier"></see> objects representing the
        ///     attributes associated with the corresponding element in the types array. The default binder does not process this
        ///     parameter.
        /// </param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one property is found with the specified name
        ///     and matching the specified argument types and modifiers.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="types">types</paramref> is multidimensional.   -or-
        ///     <paramref name="modifiers">modifiers</paramref> is multidimensional.   -or-
        ///     <paramref name="types">types</paramref> and <paramref name="modifiers">modifiers</paramref> do not have the same
        ///     length.
        /// </exception>
        /// <exception cref="T:System.NullReferenceException">An element of <paramref name="types">types</paramref> is null.</exception>
        public DynamicProperty GetProperty(
            string name,
            Type returnType,
            Type[] types,
            ParameterModifier[] modifiers) {
            var property = Type.GetProperty(name, returnType, types, modifiers);
            return property is not null
                ? RegisterProperty(property)
                : null;
        }

        /// <summary>Searches for the specified public property whose parameters match the specified argument types.</summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="types">
        ///     An array of <see cref="T:System.Type"></see> objects representing the number, order, and type of
        ///     the parameters for the indexed property to get.   -or-   An empty array of the type
        ///     <see cref="T:System.Type"></see> (that is, Type[] types = new Type[0]) to get a property that is not indexed.
        /// </param>
        /// <returns>
        ///     An object representing the property whose parameters match the specified argument types, if found;
        ///     otherwise, null.
        /// </returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one property is found with the specified name
        ///     and matching the specified argument types.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null.   -or-
        ///     <paramref name="types">types</paramref> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="types">types</paramref> is multidimensional.</exception>
        /// <exception cref="T:System.NullReferenceException">An element of <paramref name="types">types</paramref> is null.</exception>
        public DynamicProperty GetProperty(string name, params Type[] types) {
            var property = Type.GetProperty(name, types);
            return property is not null
                ? RegisterProperty(property)
                : null;
        }
        #endregion


        #region Accessors
        /// <summary>
        ///     When overridden in a derived class, searches for the accessors of the current <see cref="T:System.Type"></see>
        ///     , using the specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>
        ///     An array of <see cref="T:System.Reflection.AccessorInfo"></see> objects representing all accessors of the
        ///     current <see cref="T:System.Type"></see> that match the specified binding constraints.   -or-   An empty array of
        ///     type <see cref="T:System.Reflection.AccessorInfo"></see>, if the current <see cref="T:System.Type"></see> does not
        ///     have accessors, or if none of the accessors match the binding constraints.
        /// </returns>
        public IEnumerable<DynamicAccessor> GetAccessors(BindingFlags bindingAttr) {
            var properties = GetProperties(bindingAttr);
            var fields = GetFields(bindingAttr);

            return properties.Concat<DynamicAccessor>(fields);
        }

        /// <summary>Searches for the accessor with the specified name.</summary>
        /// <param name="name">The string containing the name of the accessor to get.</param>
        /// <returns>An object representing the accessor with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one accessor is found with the specified name.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        public DynamicAccessor GetAccessor(string name) {
            return (DynamicAccessor)GetProperty(name) ?? GetField(name);
        }

        /// <summary>Searches for the specified accessor, using the specified binding constraints.</summary>
        /// <param name="name">The string containing the name of the accessor to get.</param>
        /// <param name="bindingAttr">
        ///     A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"></see> that
        ///     specify how the search is conducted.   -or-   Zero, to return null.
        /// </param>
        /// <returns>An object representing the accessor that matches the specified requirements, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">
        ///     More than one accessor is found with the specified name
        ///     and matching the specified binding constraints.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name">name</paramref> is null.</exception>
        public DynamicAccessor GetAccessor(string name, BindingFlags bindingAttr) {
            return (DynamicAccessor)GetProperty(name, bindingAttr) ?? GetField(name, bindingAttr);
        }

        /// <summary>Searches for the accessor with the specified name and return type.</summary>
        /// <param name="name">The string containing the name of the accessor to get.</param>
        /// <param name="returnType">The return type of the accessor.</param>
        /// <returns>An object representing the accessor with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one accessor is found with the specified name.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="name">name</paramref> is null, or
        ///     <paramref name="returnType">returnType</paramref> is null.
        /// </exception>
        public DynamicAccessor GetAccessor(string name, Type returnType) {
            var accessor = GetProperty(name, returnType);
            if(accessor is not null)
                return accessor;

            var field = GetField(name);
            if(field == null)
                return null;

            return field.ValueType == returnType
                ? field
                : null;
        }
        #endregion

        #region Resolve

        /// <summary>
        ///     Tries to resolve the corresponding dynamic constructor
        /// </summary>
        public DynamicMember Resolve(MemberInfo memberInfo) {
            return memberInfo switch {
                ConstructorInfo constructorInfo => Resolve(constructorInfo),
                FieldInfo fieldInfo => Resolve(fieldInfo),
                MethodInfo methodInfo => Resolve(methodInfo),
                PropertyInfo propertyInfo => Resolve(propertyInfo),
                _ => null
            };
        }

        /// <summary>
        ///     Tries to resolve the corresponding dynamic constructor
        /// </summary>
        public DynamicCtor Resolve(ConstructorInfo ctor)
        {
            if (ctor.ReflectedType != Type)
                throw new ArgumentException("The ctor is reflected by another type");
            return RegisterConstructor(ctor);
        }

        /// <summary>
        ///     Tries to resolve the corresponding dynamic field
        /// </summary>
        public DynamicField Resolve(FieldInfo field) {
            if(field.ReflectedType != Type)
                throw new ArgumentException("The field is reflected by another type");
            return RegisterField(field);
        }

        /// <summary>
        ///     Tries to resolve the corresponding dynamic property
        /// </summary>
        public DynamicProperty Resolve(PropertyInfo property)
        {
            if (property.ReflectedType != Type)
                throw new ArgumentException("The property is reflected by another type");
            return RegisterProperty(property);
        }

        /// <summary>
        ///     Tries to resolve the corresponding dynamic method
        /// </summary>
        public DynamicInvoke Resolve(MethodInfo method)
        {
            if (method.ReflectedType != Type)
                throw new ArgumentException("The method is reflected by another type");
            return RegisterMethod(method);
        }

        #endregion

        #region Helper
        public DynamicCtor CreateDefaultConstructor() {
            var defaultCtor = Type.GetConstructor(Type.EmptyTypes);
            if(defaultCtor is not null)
                return RegisterConstructor(defaultCtor);
            return Type.IsValueType
                ? Type.CreateDefaultCtor()
                : null;
        }

        private DynamicCtor[] CreateConstructors() {
            var dynamicCtors = Type.GetConstructors(AllBindingFlags)
               .Select(RegisterConstructor)
               .SkipNull()
               .ToList();
            if(Type.IsValueType && (dynamicCtors.Any(c => c.IsDefault) == false))
                dynamicCtors.Add(DefaultConstructor);
            return dynamicCtors.ToArray();
        }

        private DynamicInvoke[] CreateMethods() {
            return Type.GetMethods(AllBindingFlags)
               .Select(RegisterMethod)
               .SkipNull()
               .ToArray();
        }

        private DynamicField[] CreateFields() {
            return Type.GetFields(AllBindingFlags)
               .Select(RegisterField)
               .SkipNull()
               .ToArray();
        }

        private DynamicProperty[] CreateProperties() {
            return Type.GetProperties(AllBindingFlags)
               .Select(RegisterProperty)
               .SkipNull()
               .ToArray();
        }

        private DynamicCtor RegisterConstructor(ConstructorInfo constructorInfo) {
            return (DynamicCtor)_memberMap.GetOrAdd(constructorInfo, c => ((ConstructorInfo)c).CreateDynamicCtor());
        }

        private DynamicInvoke RegisterMethod(MethodInfo methodInfo) {
            return (DynamicInvoke)_memberMap.GetOrAdd(methodInfo, c => ((MethodInfo)c).CreateDynamicInvoke());
        }

        private DynamicField RegisterField(FieldInfo fieldInfo) {
            return (DynamicField)_memberMap.GetOrAdd(fieldInfo, c => ((FieldInfo)c).CreateDynamicField());
        }

        private DynamicProperty RegisterProperty(PropertyInfo propertyInfo) {
            return (DynamicProperty)_memberMap.GetOrAdd(propertyInfo, c => ((PropertyInfo)c).CreateDynamicProperty());
        }

        private static bool TypeArrayEquals(Type[] left, Type[] right) {
            if(left.Length != right.Length)
                return false;
            for(var i = 0; i < left.Length; i++) {
                if(left[i] != right[i])
                    return false;
            }

            return true;
        }
        #endregion
    }
}
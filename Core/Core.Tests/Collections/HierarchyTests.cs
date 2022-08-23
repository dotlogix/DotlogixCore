using System;
using System.Linq;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;
using NUnit.Framework;

namespace Core.Tests.Collections {
    [TestFixture]
    public class HierarchyTests {
        private const string DefinitivelyNotExistingKey = "definitively_not_existing_header";

        private class HierarchyData {
            /// <inheritdoc />
            public HierarchyData(string key, string parentKey) {
                Key = key;
                ParentKey = parentKey;
            }

            public string Key { get; }
            public string ParentKey { get; }
        }


        [SetUp]
        public void Setup() {
        }

        [Test]
        public void Hierarchy_RootItem_AssertBasicProps() {
            var hierarchy = CreateRootHierarchy();
            Assert.That(hierarchy.IsRoot, Is.True);
            Assert.That(hierarchy.HasAncestor, Is.False);
            Assert.That(hierarchy.Ancestor, Is.Null);
            Assert.That(hierarchy.Root, Is.SameAs(hierarchy));

            // because its empty
            Assert.That(hierarchy.Count, Is.EqualTo(0));
            Assert.That(hierarchy.Key, Is.EqualTo("root"));
            Assert.That(hierarchy.Value, Is.Null);
        }

        #region AddChild

        [Test]
        public void AddChild_NonRootItem_AssertBasicProps() {
            var hierarchy = CreateRootHierarchy();
            var hierarchyData = CreateDummyData();

            var child = hierarchy.AddChild("child", hierarchyData);
            Assert.That(child.IsRoot, Is.False);
            Assert.That(child.HasAncestor, Is.True);
            Assert.That(child.Ancestor, Is.SameAs(hierarchy));
            Assert.That(child.Root, Is.SameAs(hierarchy));

            // because its empty
            Assert.That(child.Count, Is.EqualTo(0));
            Assert.That(child.Key, Is.EqualTo("child"));
            Assert.That(child.Value, Is.SameAs(hierarchyData));

            Assert.That(hierarchy.Count, Is.EqualTo(1));
        }


        [Test]
        public void AddChild_NullKey_ThrowsArgumentException() {
            var hierarchy = CreateRootHierarchy();
            var hierarchyData = CreateDummyData();
            Assert.Throws<ArgumentNullException>(() => hierarchy.AddChild(null, hierarchyData));
        }

        [Test]
        public void AddChild_SameKey_ThrowsArgumentException() {
            var hierarchy = CreateRootHierarchy();
            var hierarchyData = CreateDummyData();

            hierarchy.AddChild("child", hierarchyData);
            Assert.Throws<ArgumentException>(() => hierarchy.AddChild("child", hierarchyData));
        }

        [Test]
        public void AddChild_Hierarchy_NonRootItem_AssertBasicProps() {
            var hierarchy = CreateRootHierarchy();
            var hierarchyData = CreateDummyData();
            var child = new Hierarchy<string, HierarchyData>("child", hierarchyData);
            hierarchy.AddChild(child);

            Assert.That(child.IsRoot, Is.False);
            Assert.That(child.HasAncestor, Is.True);
            Assert.That(child.Ancestor, Is.SameAs(hierarchy));
            Assert.That(child.Root, Is.SameAs(hierarchy));

            // because its empty
            Assert.That(child.Count, Is.EqualTo(0));
            Assert.That(child.Key, Is.EqualTo("child"));
            Assert.That(child.Value, Is.SameAs(hierarchyData));

            Assert.That(hierarchy.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddChild_Hierarchy_NullChild_ThrowsArgumentException() {
            var hierarchy = CreateRootHierarchy();
            Assert.Throws<ArgumentNullException>(() => hierarchy.AddChild(null));
        }

        [Test]
        public void AddChild_Hierarchy_ItemWithOtherAncestor_ThrowsArgumentException() {
            var hierarchy = CreateRootHierarchy();
            var hierarchy2 = CreateRootHierarchy();
            var h2Child = hierarchy2.AddChild("child", CreateDummyData());
            
            Assert.Throws<ArgumentException>(() => { hierarchy.AddChild(h2Child); });
        }

        private static HierarchyData CreateDummyData(string key = "child", string parentKey = "root") {
            return new HierarchyData(key, parentKey);
        }
        #endregion

        #region Get

        [Test]
        public void GetChild_ExistingKey_ReturnsCorrectValue() {
            var hierarchy = CreateRootHierarchy();
            var childData = CreateDummyData();
            var childData2 = CreateDummyData("child2");
            hierarchy.AddChild(childData.Key, childData);
            hierarchy.AddChild(childData2.Key, childData2);

            Assert.That(hierarchy.GetChild(childData.Key), Is.Not.Null);
            Assert.That(hierarchy.GetChild(childData.Key).Value, Is.SameAs(childData));

            Assert.That(hierarchy.GetChild(childData2.Key), Is.Not.Null);
            Assert.That(hierarchy.GetChild(childData2.Key).Value, Is.SameAs(childData2));
        }

        [Test]
        public void GetChild_NullKey_ThrowsArgumentException() {
            var hierarchy = CreateRootHierarchy();
            Assert.Throws<ArgumentNullException>(() => hierarchy.GetChild(null));
        }

        [Test]
        public void GetChild_NonExistingKey_ReturnsNull() {
            var hierarchy = CreateRootHierarchy();
            Assert.That(hierarchy.GetChild(DefinitivelyNotExistingKey), Is.Null);
        }

        #endregion


        #region Get

        [Test]
        public void Ancestors_ReturnsCorrectAncestors() {
            var hierarchyData = new[] {
                new HierarchyData("0", null),
                new HierarchyData("1", null),
                new HierarchyData("2", null),
                new HierarchyData("0.0", "0"),
                new HierarchyData("0.1", "0"),
                new HierarchyData("0.2", "0"),
                new HierarchyData("1.0", "1"),
                new HierarchyData("1.0.1", "1.0"),
            };

            var hierarchy = hierarchyData.ToHierarchy(d => d.Key, d => d.ParentKey);
            var child0 = hierarchy.GetChild("0");
            var child1 = hierarchy.GetChild("1");
            var child2 = hierarchy.GetChild("2");
            Assert.That(child0.Ancestor,Is.SameAs(hierarchy));
            Assert.That(child1.Ancestor,Is.SameAs(hierarchy));
            Assert.That(child2.Ancestor,Is.SameAs(hierarchy));

            var child0Children = child0.Children().ToArray();
            var child1Children = child1.Children().ToArray();
            var child11Children = child1Children[0].Children().ToArray();
            var child2Children = child2.Children().ToArray();

            Assert.That(child0.Ancestor, Is.SameAs(hierarchy));
            Assert.That(child1.Ancestor, Is.SameAs(hierarchy));
            Assert.That(child2.Ancestor, Is.SameAs(hierarchy));

            Assert.That(child0.Ancestor, Is.SameAs(hierarchy));
            Assert.That(child1Children[0].Ancestor, Is.SameAs(child1));
            Assert.That(child11Children[0].Ancestor, Is.SameAs(child1Children[0]));

        }

        #endregion

        private static Hierarchy<string, HierarchyData> CreateRootHierarchy() {
            return new Hierarchy<string, HierarchyData>("root", null);
        }
    }
}
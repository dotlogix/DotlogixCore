// ==================================================
// Copyright 2016(C) , DotLogix
// File:  NodeIoStateMachine.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  24.10.2017
// LastEdited:  25.10.2017
// ==================================================

#region
#endregion

namespace DotLogix.Core.Nodes.Io {
    public class NodeIoStateMachine {
        public NodeIoState PreviousState { get; private set; }
        public NodeIoState CurrentState { get; private set; }
        public NodeIoOpCodes AllowedOperations { get; private set; }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public NodeIoStateMachine(NodeIoState startingState, NodeIoOpCodes allowedOperations) {
            PreviousState = NodeIoState.None;
            CurrentState = startingState;
            AllowedOperations = allowedOperations;
        }

        public bool GoToState(NodeIoOperation operation) {
            return GoToState(operation.NextState, operation.OpCode, operation.AllowedNextOpCodes);
        }

        public bool GoToState(NodeIoState nextState, NodeIoOpCodes withOperation, NodeIoOpCodes allowedNextOperations) {
            if(IsAllowedOperation(withOperation) == false)
                return false;

            PreviousState = CurrentState;
            CurrentState = nextState;
            AllowedOperations = allowedNextOperations;
            return true;
        }

        public bool IsAllowedOperation(NodeIoOpCodes withOperation) {
            return (AllowedOperations & withOperation) != 0;
        }
    }
}

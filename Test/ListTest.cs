﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ListTest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace Test {
    [TestClass]
    public class ListTest {
        [TestMethod]
        public void TestInsertSorted() {
            var list = new List<int> {0, 1, 2, 3, 5, 6};
            list.InsertSorted(4);
            Assert.AreEqual(4, list[4]);
        }

        [TestMethod]
        public void TestInsertSortedWithSelector() {
            var list = new List<int> {0, 1, 2, 3, 4, 5, 6};
            list.InsertSorted(4, t => t);
            Assert.AreEqual(4, list[4]);
            Assert.AreEqual(4, list[5]);
        }

        [TestMethod]
        public void TestSortedCollection() {
            var list = new SortedCollection<int> {5, 2, 1, 4, 3, 0};
            for(var i = 0; i < 6; i++)
                Assert.AreEqual(i, list[i]);
            list.Add(4);
            Assert.AreEqual(4, list[4]);
            Assert.AreEqual(4, list[5]);
        }
    }
}

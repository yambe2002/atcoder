using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void UnionFind()
        {
            var uf = new UnionFind(10);

            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsFalse(uf.Same(1, 5));
            Assert.IsFalse(uf.Same(5, 9));
            Assert.IsFalse(uf.Same(1, 9));
            Assert.IsFalse(uf.Same(0, 1));
            Assert.IsFalse(uf.Same(3, 4));
            Assert.IsFalse(uf.Same(3, 5));
            Assert.IsFalse(uf.Same(7, 8));
            Assert.IsFalse(uf.Same(4, 7));
            Assert.IsFalse(uf.Same(0, 7));
            Assert.IsFalse(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ 0B123B4567 ]
            uf.Unite(1, 5);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsFalse(uf.Same(5, 9));
            Assert.IsFalse(uf.Same(1, 9));
            Assert.IsFalse(uf.Same(0, 1));
            Assert.IsFalse(uf.Same(3, 4));
            Assert.IsFalse(uf.Same(3, 5));
            Assert.IsFalse(uf.Same(7, 8));
            Assert.IsFalse(uf.Same(4, 7));
            Assert.IsFalse(uf.Same(0, 7));
            Assert.IsFalse(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ 0B123B456B ]
            uf.Unite(5, 9);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsFalse(uf.Same(0, 1));
            Assert.IsFalse(uf.Same(3, 4));
            Assert.IsFalse(uf.Same(3, 5));
            Assert.IsFalse(uf.Same(7, 8));
            Assert.IsFalse(uf.Same(4, 7));
            Assert.IsFalse(uf.Same(0, 7));
            Assert.IsFalse(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ 0B1AAB456B ]
            uf.Unite(3, 4);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsFalse(uf.Same(0, 1));
            Assert.IsTrue(uf.Same(3, 4));
            Assert.IsFalse(uf.Same(3, 5));
            Assert.IsFalse(uf.Same(7, 8));
            Assert.IsFalse(uf.Same(4, 7));
            Assert.IsFalse(uf.Same(0, 7));
            Assert.IsFalse(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ 0B1AAB4CCB ]
            uf.Unite(7, 8);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsFalse(uf.Same(0, 1));
            Assert.IsTrue(uf.Same(3, 4));
            Assert.IsFalse(uf.Same(3, 5));
            Assert.IsTrue(uf.Same(7, 8));
            Assert.IsFalse(uf.Same(4, 7));
            Assert.IsFalse(uf.Same(0, 7));
            Assert.IsFalse(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ 0A1AAA4CCA ]
            uf.Unite(1, 3);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsFalse(uf.Same(0, 1));
            Assert.IsTrue(uf.Same(3, 4));
            Assert.IsTrue(uf.Same(3, 5));
            Assert.IsTrue(uf.Same(7, 8));
            Assert.IsFalse(uf.Same(4, 7));
            Assert.IsFalse(uf.Same(0, 7));
            Assert.IsTrue(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ CA1AAA4CCA ]
            uf.Unite(0, 7);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsFalse(uf.Same(0, 1));
            Assert.IsTrue(uf.Same(3, 4));
            Assert.IsTrue(uf.Same(3, 5));
            Assert.IsTrue(uf.Same(7, 8));
            Assert.IsFalse(uf.Same(4, 7));
            Assert.IsTrue(uf.Same(0, 7));
            Assert.IsTrue(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ AA1AAA4AAA ]
            uf.Unite(0, 1);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsTrue(uf.Same(0, 1));
            Assert.IsTrue(uf.Same(3, 4));
            Assert.IsTrue(uf.Same(3, 5));
            Assert.IsTrue(uf.Same(7, 8));
            Assert.IsTrue(uf.Same(4, 7));
            Assert.IsTrue(uf.Same(0, 7));
            Assert.IsTrue(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsFalse(uf.Same(2, 6));

            // [ AABAAABAAA ]
            uf.Unite(2, 6);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsTrue(uf.Same(0, 1));
            Assert.IsTrue(uf.Same(3, 4));
            Assert.IsTrue(uf.Same(3, 5));
            Assert.IsTrue(uf.Same(7, 8));
            Assert.IsTrue(uf.Same(4, 7));
            Assert.IsTrue(uf.Same(0, 7));
            Assert.IsTrue(uf.Same(1, 3));
            Assert.IsFalse(uf.Same(1, 2));
            Assert.IsFalse(uf.Same(5, 6));
            Assert.IsTrue(uf.Same(2, 6));

            // [ AAAAAAAAAA ]
            uf.Unite(2, 3);
            Assert.IsTrue(uf.Same(1, 1));
            Assert.IsTrue(uf.Same(1, 5));
            Assert.IsTrue(uf.Same(5, 9));
            Assert.IsTrue(uf.Same(1, 9));
            Assert.IsTrue(uf.Same(0, 1));
            Assert.IsTrue(uf.Same(3, 4));
            Assert.IsTrue(uf.Same(3, 5));
            Assert.IsTrue(uf.Same(7, 8));
            Assert.IsTrue(uf.Same(4, 7));
            Assert.IsTrue(uf.Same(0, 7));
            Assert.IsTrue(uf.Same(1, 3));
            Assert.IsTrue(uf.Same(1, 2));
            Assert.IsTrue(uf.Same(5, 6));
            Assert.IsTrue(uf.Same(2, 6));
        }
    }
}

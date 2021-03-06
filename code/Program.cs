﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Program
    {
        static IO.StreamScanner _scanner = new IO.StreamScanner(Console.OpenStandardInput());

        static void Main(string[] args)
        {
            var t = _scanner.Integer();
        }       
    }

    #region library

    public class UnionFind
    {
        List<int> _parent;

        public UnionFind(int N)
        {
            _parent = Enumerable.Range(0, N).Select(i => i).ToList();
        }

        public int Root(int x)
        {
            return _parent[x] == x ? x : _parent[x] = Root(_parent[x]);
        }

        public bool Same(int x, int y)
        {
            return Root(x) == Root(y);
        }

        public void Unite(int x, int y)
        {
            _parent[Root(x)] = Root(y);
        }
    }

    public class Rmq
    {
        const int MAX_N = 1 << 21;
        int _n;
        long[] _dat = new long[2 * MAX_N - 1];

        public void Init(int n)
        {
            _n = 1;
            while (_n < n) _n *= 2;
            for (int i = 0; i < _dat.Length; i++) _dat[i] = long.MaxValue;
        }

        public void Update(int k, long a)
        {
            k += _n - 1;

            _dat[k] = a;
            while (k > 0)
            {
                k = (k - 1) / 2;
                _dat[k] = Math.Min(_dat[k * 2 + 1], _dat[k * 2 + 2]);
            }
        }

        public long Query(int a, int b)
        {
            return Query(a, b, 0, 0, _n);
        }

        long Query(int a, int b, int k, int l, int r)
        {
            if (r <= a || b <= l) return long.MaxValue;
            if (a <= l && r <= b) return _dat[k];
            else
            {
                var vl = Query(a, b, k * 2 + 1, l, (l + r) / 2);
                var vr = Query(a, b, k * 2 + 2, (l + r) / 2, r);
                return Math.Min(vl, vr);
            }
        }
    }

    public class Lazy_SegTree
    {
        long MOD = 1000000007L;
        const int SIZE = 1 << 17;
        public List<long> Seg;

        List<long> _lazy;

        public Lazy_SegTree()
        {
            Seg = Enumerable.Range(0, SIZE * 2).Select(v => (long)0).ToList();
            _lazy = Enumerable.Range(0, SIZE * 2).Select(v => (long)0).ToList();
        }

        //遅延情報の適用方法
        void LazyEvaluate(int k, int l, int r)
        {
            if (_lazy[k] != 0)
            {
                Seg[k] += _lazy[k];//区間[l,r)にすべて同じ値を追加することになっていて、segには合計値が入っているので、加える値を足す
                Seg[k] %= MOD;
                if (r - l > 1)
                {
                    _lazy[k * 2 + 1] += _lazy[k];//遅延を左の子に伝搬
                    _lazy[k * 2 + 2] += _lazy[k];//遅延を右の子に伝搬
                }
                _lazy[k] = 0;//ノードkは伝搬完了
            }
        }

        void Update(int a, int b, int k, int l, int r, long x)
        {
            LazyEvaluate(k, l, r);
            if (r <= a || b <= l) return;
            if (a <= l && r <= b)
            {
                _lazy[k] += x; //加える
                LazyEvaluate(k, l, r);
            }
            else
            {
                Update(a, b, k * 2 + 1, l, (l + r) / 2, x);
                Update(a, b, k * 2 + 2, (l + r) / 2, r, x);
                Seg[k] = Seg[k * 2 + 1] + Seg[k * 2 + 2]; //区間の合計
                Seg[k] %= MOD;
            }
        }

        long Query(int a, int b, int k, int l, int r)
        {
            LazyEvaluate(k, l, r);
            if (r <= a || b <= l) return 0;//合計に影響のないもの
            if (a <= l && r <= b) return Seg[k];
            var x = Query(a, b, k * 2 + 1, l, (l + r) / 2);
            var y = Query(a, b, k * 2 + 2, (l + r) / 2, r);
            return x + y; //左右の合計を
        }

        //update(a,b,x) := [a,b)を全てxを加える
        public void Update(int a, int b, long x)
        {
            Update(a, b, 0, 0, SIZE, x);
        }

        //query(a,b) := [a,b)に対する合計値を求める
        public long Query(int a, int b)
        {
            return Query(a, b, 0, 0, SIZE);
        }
    }

    public class PriorityQueue<T> where T : IComparable
    {
        private IComparer<T> _comparer = null;
        private int _type = 0;

        private T[] _heap;
        private int _sz = 0;

        private int _count = 0;

        /// <summary>
        /// Priority Queue with custom comparer
        /// </summary>
        public PriorityQueue(IComparer<T> comparer)
        {
            _heap = new T[128];
            _comparer = comparer;
        }

        /// <summary>
        /// Priority queue
        /// </summary>
        /// <param name="type">0: asc, 1:desc</param>
        public PriorityQueue(int type = 0)
        {
            _heap = new T[128];
            _type = type;
        }

        private int Compare(T x, T y)
        {
            if (_comparer != null) return _comparer.Compare(x, y);
            return _type == 0 ? x.CompareTo(y) : y.CompareTo(x);
        }

        public void Push(T x)
        {
            _count++;
            if (_count > _heap.Length)
            {
                var newheap = new T[_heap.Length * 2];
                for (int n = 0; n < _heap.Length; n++) newheap[n] = _heap[n];
                _heap = newheap;
            }

            //node number
            var i = _sz++;

            while (i > 0)
            {
                //parent node number
                var p = (i - 1) / 2;

                if (Compare(_heap[p], x) <= 0) break;

                _heap[i] = _heap[p];
                i = p;
            }

            _heap[i] = x;
        }

        public T Pop()
        {
            _count--;

            T ret = _heap[0];
            T x = _heap[--_sz];

            int i = 0;
            while (i * 2 + 1 < _sz)
            {
                //children
                int a = i * 2 + 1;
                int b = i * 2 + 2;

                if (b < _sz && Compare(_heap[b], _heap[a]) < 0) a = b;

                if (Compare(_heap[a], x) >= 0) break;

                _heap[i] = _heap[a];
                i = a;
            }

            _heap[i] = x;

            return ret;
        }

        public int Count()
        {
            return _count;
        }

        public T Peek()
        {
            return _heap[0];
        }

        public bool Contains(T x)
        {
            for (int i = 0; i < _sz; i++) if (x.Equals(_heap[i])) return true;
            return false;
        }

        public void Clear()
        {
            while (this.Count() > 0) this.Pop();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var ret = new List<T>();

            while (this.Count() > 0)
            {
                ret.Add(this.Pop());
            }

            foreach (var r in ret)
            {
                this.Push(r);
                yield return r;
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[_sz];
            int i = 0;

            foreach (var r in this)
            {
                array[i++] = r;
            }

            return array;
        }
    }

    // <summary>
    // Self-Balancing Binary Search Tree
    // (using Randamized BST)
    // </summary>
    public class SB_BinarySearchTree<T> where T : IComparable
    {
        public class Node
        {
            public T Value;
            public Node LChild;
            public Node RChild;
            public int Count;     //size of the sub tree
                                  //            public int Sum;       //sum of the value of the sub tree

            public Node(T v)
            {
                Value = v;
                Count = 1;
                //                Sum = v;
            }
        }

        static Random _rnd = new Random();

        public static int Count(Node t)
        {
            return t == null ? 0 : t.Count;
        }

        //public static int Sum(Node t)
        //{
        //    return t == null ? 0 : t.Sum;
        //}

        static Node Update(Node t)
        {
            t.Count = Count(t.LChild) + Count(t.RChild) + 1;
            //            t.Sum = Sum(t.LChild) + Sum(t.RChild) + t.Value;
            return t;
        }

        public static Node Merge(Node l, Node r)
        {
            if (l == null || r == null) return l == null ? r : l;

            if ((double)Count(l) / (double)(Count(l) + Count(r)) > _rnd.NextDouble())
            //            if ((double)Count(l) / (double)(Count(l) + Count(r)) > 0.5)   //debug
            {
                l.RChild = Merge(l.RChild, r);
                return Update(l);
            }
            else
            {
                r.LChild = Merge(l, r.LChild);
                return Update(r);
            }
        }

        /// <summary>
        /// split as [0, k), [k, n)
        /// </summary>
        public static Tuple<Node, Node> Split(Node t, int k)
        {
            if (t == null) return new Tuple<Node, Node>(null, null);
            if (k <= Count(t.LChild))
            {
                var s = Split(t.LChild, k);
                t.LChild = s.Item2;
                return new Tuple<Node, Node>(s.Item1, Update(t));
            }
            else
            {
                var s = Split(t.RChild, k - Count(t.LChild) - 1);
                t.RChild = s.Item1;
                return new Tuple<Node, Node>(Update(t), s.Item2);
            }
        }

        public static Node Remove(Node t, T v)
        {
            if (Find(t, v) == null) return t;
            return RemoveAt(t, LowerBound(t, v));
        }

        public static Node RemoveAt(Node t, int k)
        {
            var s = Split(t, k);
            var s2 = Split(s.Item2, 1);
            return Merge(s.Item1, s2.Item2);
        }

        public static bool Contains(Node t, T v)
        {
            return Find(t, v) != null;
        }

        public static Node Find(Node t, T v)
        {
            while (t != null)
            {
                var cmp = t.Value.CompareTo(v);
                if (cmp > 0) t = t.LChild;
                else if (cmp < 0) t = t.RChild;
                else break;
            }
            return t;
        }

        public static Node FindByIndex(Node t, int idx)
        {
            if (t == null) return null;

            var currentIdx = Count(t) - Count(t.RChild) - 1;
            while (t != null)
            {
                if (currentIdx == idx) return t;
                if (currentIdx > idx)
                {
                    t = t.LChild;
                    currentIdx -= (Count(t == null ? null : t.RChild) + 1);
                }
                else
                {
                    t = t.RChild;
                    currentIdx += (Count(t == null ? null : t.LChild) + 1);
                }
            }

            return null;
        }

        public static int UpperBound(Node t, T v)
        {
            var torg = t;
            if (t == null) return -1;

            var ret = Int32.MaxValue;
            var idx = Count(t) - Count(t.RChild) - 1;
            while (t != null)
            {
                var cmp = t.Value.CompareTo(v);

                if (cmp > 0)
                {
                    ret = Math.Min(ret, idx);
                    t = t.LChild;
                    idx -= (Count(t == null ? null : t.RChild) + 1);
                }
                else if (cmp <= 0)
                {
                    t = t.RChild;
                    idx += (Count(t == null ? null : t.LChild) + 1);
                }
            }
            return ret == Int32.MaxValue ? Count(torg) : ret;
        }

        public static int LowerBound(Node t, T v)
        {
            var torg = t;
            if (t == null) return -1;

            var idx = Count(t) - Count(t.RChild) - 1;
            var ret = Int32.MaxValue;
            while (t != null)
            {
                var cmp = t.Value.CompareTo(v);
                if (cmp >= 0)
                {
                    if (cmp == 0) ret = Math.Min(ret, idx);
                    t = t.LChild;
                    if (t == null) ret = Math.Min(ret, idx);
                    idx -= t == null ? 0 : (Count(t.RChild) + 1);
                }
                else if (cmp < 0)
                {
                    t = t.RChild;
                    idx += (Count(t == null ? null : t.LChild) + 1);
                    if (t == null) return idx;
                }
            }
            return ret == Int32.MaxValue ? Count(torg) : ret;
        }

        public static Node Insert(Node t, T v)
        {
            var ub = LowerBound(t, v);
            return InsertByIdx(t, ub, v);
        }

        static Node InsertByIdx(Node t, int k, T v)
        {
            var s = Split(t, k);
            return Merge(Merge(s.Item1, new Node(v)), s.Item2);
        }

        public static IEnumerable<T> Enumerate(Node t)
        {
            var ret = new List<T>();
            Enumerate(t, ret);
            return ret;
        }

        static void Enumerate(Node t, List<T> ret)
        {
            if (t == null) return;
            Enumerate(t.LChild, ret);
            ret.Add(t.Value);
            Enumerate(t.RChild, ret);
        }

        // debug
        public static int GetDepth(Node t)
        {
            return t == null ? 0 : Math.Max(GetDepth(t.LChild), GetDepth(t.RChild)) + 1;
        }
    }

    /// <summary>
    /// C-like set
    /// </summary>
    public class Set<T> where T : IComparable
    {
        protected SB_BinarySearchTree<T>.Node _root;

        public T this[int idx] { get { return ElementAt(idx); } }

        public int Count()
        {
            return SB_BinarySearchTree<T>.Count(_root);
        }

        public virtual void Insert(T v)
        {
            if (_root == null) _root = new SB_BinarySearchTree<T>.Node(v);
            else
            {
                if (SB_BinarySearchTree<T>.Find(_root, v) != null) return;
                _root = SB_BinarySearchTree<T>.Insert(_root, v);
            }
        }

        public void Clear()
        {
            _root = null;
        }

        public void Remove(T v)
        {
            _root = SB_BinarySearchTree<T>.Remove(_root, v);
        }

        public bool Contains(T v)
        {
            return SB_BinarySearchTree<T>.Contains(_root, v);
        }

        public T ElementAt(int k)
        {
            var node = SB_BinarySearchTree<T>.FindByIndex(_root, k);
            if (node == null) throw new IndexOutOfRangeException();
            return node.Value;
        }

        public int Count(T v)
        {
            return SB_BinarySearchTree<T>.UpperBound(_root, v) - SB_BinarySearchTree<T>.LowerBound(_root, v);
        }

        public int LowerBound(T v)
        {
            return SB_BinarySearchTree<T>.LowerBound(_root, v);
        }

        public int UpperBound(T v)
        {
            return SB_BinarySearchTree<T>.UpperBound(_root, v);
        }

        public Tuple<int, int> EqualRange(T v)
        {
            if (!Contains(v)) return new Tuple<int, int>(-1, -1);
            return new Tuple<int, int>(SB_BinarySearchTree<T>.LowerBound(_root, v), SB_BinarySearchTree<T>.UpperBound(_root, v) - 1);
        }

        public List<T> ToList()
        {
            return new List<T>(SB_BinarySearchTree<T>.Enumerate(_root));
        }
    }

    /// <summary>
    /// C-like multiset
    /// </summary>
    public class MultiSet<T> : Set<T> where T : IComparable
    {
        public override void Insert(T v)
        {
            if (_root == null) _root = new SB_BinarySearchTree<T>.Node(v);
            else _root = SB_BinarySearchTree<T>.Insert(_root, v);
        }
    }

    public class Vect2D : IComparable<Vect2D>
    {
        public double X;
        public double Y;

        public Vect2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vect2D operator +(Vect2D v1, Vect2D v2)
        {
            return new Vect2D(DoubleUtil.Add(v1.X, v2.X), DoubleUtil.Add(v1.Y, v2.Y));
        }
        public static Vect2D operator -(Vect2D v1, Vect2D v2)
        {
            return new Vect2D(DoubleUtil.Add(v1.X, -v2.X), DoubleUtil.Add(v1.Y, -v2.Y));
        }
        public static Vect2D operator *(Vect2D v1, Vect2D v2)
        {
            return new Vect2D(v1.X * v2.X - v1.Y * v2.Y, v1.X * v2.Y + v2.X * v1.Y);
        }
        public static Vect2D operator *(Vect2D v, double d)
        {
            return new Vect2D(v.X * d, v.Y * d);
        }
        public static Vect2D operator /(Vect2D v, double d)
        {
            return new Vect2D(v.X / d, v.Y / d);
        }

        /// <summary>
        /// Dot product of two vectors: O -> this and O -> other
        ///  a dot b = |a| |b| cos(theta) = ax bx + ax by
        ///  zero if two vectors run orthogonally
        /// </summary>
        public double Dot(Vect2D other)
        {
            return DoubleUtil.Add(this.X * other.X, this.Y * other.Y);
        }

        /// <summary>
        /// Cross(det) product of two vectors: O -> this and O -> other
        ///  a x b = |a| |b| sin(theta) = ax by - ay bx
        ///  zero if two vectors run parallelly
        /// </summary>
        public double Cross(Vect2D other)
        {
            return DoubleUtil.Add(this.X * other.Y, -this.Y * other.X);
        }

        /// <summary>
        /// crosssing point of line p1-p2 and q1-q2
        /// </summary>
        public static Vect2D Intersect(Vect2D p1, Vect2D p2, Vect2D q1, Vect2D q2)
        {
            return p1 + (p2 - p1) * ((q2 - q1).Cross(q1 - p1) / (q2 - q1).Cross(p2 - p1));
        }

        public static bool HasIntersect(Vect2D p1, Vect2D p2, Vect2D q1, Vect2D q2)
        {
            ////do edges "this" and "other" intersect?
            if (Math.Min(p1.X, p2.X) > Math.Max(q1.X, q2.X)) return false;
            if (Math.Max(p1.X, p2.X) < Math.Min(q1.X, q2.X)) return false;
            if (Math.Min(p1.Y, p2.Y) > Math.Max(q1.Y, q2.Y)) return false;
            if (Math.Max(p1.Y, p2.Y) < Math.Min(q1.Y, q2.Y)) return false;

            var pVect = (p2 - p1);
            var qVect = (q2 - q1);

            int den = (int)(qVect.Y * pVect.X - qVect.X * pVect.Y);
            int num1 = (int)(qVect.X * (p1.Y - q1.Y) - qVect.Y * (p1.X - q1.X));
            int num2 = (int)(pVect.X * (p1.Y - q1.Y) - pVect.Y * (p1.X - q1.X));

            var pNorm = pVect.Norm();
            var qNorm = qVect.Norm();

            //parallel edges
            if (den == 0)
            {
                if (Math.Min(Dist2(p1, p2, q1, q2), Dist2(q1, q2, p1, p2)) > 0)
                    return false;

                //on the same line - "not intersect" only if one of the vertices is common,
                //and the other doesn't belong to the line
                if ((p1 == q1 && DoubleUtil.Eq(Vect2D.Dist(p2, q2), pNorm + qNorm)) ||
                    (p1 == q2 && DoubleUtil.Eq(Vect2D.Dist(p2, q1), pNorm + qNorm)) ||
                    (p2 == q1 && DoubleUtil.Eq(Vect2D.Dist(p1, q2), pNorm + qNorm)) ||
                    (p2 == q2 && DoubleUtil.Eq(Vect2D.Dist(p1, q1), pNorm + qNorm)))
                    return false;
                return true;
            }

            //common vertices
            if (p1 == q1 || p1 == q2 || p2 == q1 || p2 == q2)
                return false;

            double u1 = (double)num1 / den;
            double u2 = (double)num2 / den;
            if (u1 < 0 || u1 > 1 || u2 < 0 || u2 > 1)
                return false;
            return true;
        }

        // dist between line p1-p2 and q
        public static double Dist(Vect2D p1, Vect2D p2, Vect2D q)
        {
            var vect = p2 - p1;
            //distance from p to the edge
            if (vect.Dot(q - p1) <= 0)
                return q.Dist(p1);         //from p to p1
            if (vect.Dot(q - p2) >= 0)
                return q.Dist(p2);         //from p to p2
            //distance to the line itself
            return Math.Abs(-vect.Y * q.X + vect.X * q.Y + p1.X * p2.Y - p1.Y * p2.X) / vect.Norm();
        }

        //distance from the closest of the endpoints of line p1-p2 to line q1-q2
        public static double Dist2(Vect2D p1, Vect2D p2, Vect2D q1, Vect2D q2)
        {
            return Math.Min(Dist(p1, p2, q1), Dist(p1, p2, q2));
        }

        public static bool operator ==(Vect2D x, Vect2D y)
        {
            var ret = (DoubleUtil.Eq(x.X, y.X) && DoubleUtil.Eq(x.Y, y.Y));
            return ret;
        }

        public static bool operator !=(Vect2D x, Vect2D y)
        {
            return !(x == y);
        }

        public double Norm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public double Dist(Vect2D other)
        {
            return (this - other).Norm();
        }

        public static double Dist(Vect2D v1, Vect2D v2)
        {
            return v1.Dist(v2);
        }

        public static int Ccw(Vect2D a, Vect2D b, Vect2D c)
        {
            b -= a;
            c -= a;
            if (b.Cross(c) > 0) return 1;   //counter clockwise
            if (b.Cross(c) < 0) return -1;  //clockwise
            if (b.Dot(c) < 0) return 2;     //c--a--b on line
            if (b.Norm() < c.Norm()) return -2; //a--b--c on line
            return 0;
        }

        //ger radian angle of vector A and B
        public static double GetAngleOf2Vectors(Vect2D A, Vect2D B)
        {
            double length_A = get_vector_length(A);
            double length_B = get_vector_length(B);

            double cos_sita = A.Dot(B) / (length_A * length_B);

            double sita = Math.Acos(cos_sita);

            //degree in 0^180
            //sita = sita * 180.0 / PI;

            return sita;
        }

        static double get_vector_length(Vect2D v)
        {
            return Math.Pow((v.X * v.X) + (v.Y * v.Y), 0.5);
        }

        public override bool Equals(object obj)
        {
            var d = obj as Vect2D;
            return d != null &&
                   X == d.X &&
                   Y == d.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static List<Vect2D> GetConvexSet(List<Vect2D> ps)
        {
            ps.Sort();
            var convex = Enumerable.Range(0, ps.Count() * 2).Select(v => (Vect2D)null).ToList();

            var k = 0;
            for (var i = 0; i < ps.Count(); i++)
            {
                while (2 <= k && (convex[k - 1] - convex[k - 2]).Cross(ps[i] - convex[k - 1]) <= 0)
                    k--;
                convex[k++] = ps[i];
            }

            for (int i = ps.Count() - 2, t = k; 0 <= i; i--)
            {
                while (t < k && (convex[k - 1] - convex[k - 2]).Cross(ps[i] - convex[k - 1]) <= 0)
                    k--;
                convex[k++] = ps[i];
            }

            return convex.Take(k - 1).ToList();
        }

        public int CompareTo(Vect2D other)
        {
            if (this.X != other.X) return this.X.CompareTo(other.X);
            return this.Y.CompareTo(other.Y);
        }
    }

    public static class DoubleUtil
    {
        public static double EPS = 1e-10;

        public static double Add(double a, double b)
        {
            if (Math.Abs(a + b) < EPS * (Math.Abs(a) + Math.Abs(b))) return 0;
            return a + b;
        }

        public static bool Eq(double a, double b)
        {
            return Math.Abs(a - b) < 1e-9;
        }
    }

    #endregion

    #region library_Math

    public static class MyMath
    {
        // m種類から重複なくn個を選ぶ組み合わせ
        public static Int64 GetMcn(int m, int n)
        {
            Int64 val;
            if (m < n) return 0;
            n = Math.Min(n, m - n);
            if (n == 0) val = 1;
            else val = GetMcn(m - 1, n - 1) * m / n;
            return val;
        }

        // m種類から重複なくn個を選んで並べる組み合わせ
        public static Int64 GetMpn(int m, int n)
        {
            if (n > m) return 0L;
            Int64 ret = m;
            for (int i = 0; i < n - 1; i++)
            {
                ret *= (m - i - 1);
            }
            return ret;
        }

        // m種類から重複を許してn個を選ぶ組み合わせ（順番は無視）
        public static Int64 GetMhn(int m, int n)
        {
            return GetMcn(m + n - 1, n);
        }

        static Int64[] fact = new Int64[500005];
        static Int64[] inv = new Int64[500005];

        /// <summary>
        /// FOR TEST - mCn % p (p should be prime number)
        /// </summary>
        public static Int64 GetMcn_p_Simple(Int64 m, Int64 n, Int64 p)
        {
            // m! / ( n! ( m - n )! )
            return fact[m] * inv[m - n] % p * inv[n] % p;
        }

        /// <summary>
        ///  mCn % p (p should be prime number)
        ///  use Lucas's theorem
        ///   - need pre-calculation using the mod
        /// </summary>
        public static Int64 GetMcn_p(Int64 m, Int64 n, Int64 p)
        {
            if (!(0 <= n && n <= m)) return 0;
            Int64 ret = 1;
            for (; n > 0; m /= p, n /= p)
            {
                Int64 n0 = m % p, k0 = n % p;
                if (n0 < k0) return 0;
                ret = ret * GetMcn_p_Simple(n0, k0, p);
                ret %= p;
            }
            return ret;
        }

        /// <summary>
        ///  mCn % p (p should be prime number)
        ///  use Lucas's theorem (No pre-calculation needed)
        /// </summary>
        public static Int64 GetMcn_p_NoPrecalc(int m, int n, int p)
        {
            if (p < 2) return GetMcn(m, n);

            var dm1 = m / p;
            var dm2 = m % p;
            var dn1 = n / p;
            var dn2 = n % p;

            if ((dm2 < dn2) || (dm1 < dn1)) return 0;

            return GetMcn(dm1, dn1) * GetMcn(dm2, dn2) % p;
        }

        public static void Precal_FactAndInv(Int64 mod)
        {
            fact[0] = 1;
            inv[0] = ModInv(1, mod);

            for (Int64 i = 1; i < 500005; i++)
            {
                fact[i] = (fact[i - 1] * i) % mod;
                inv[i] = ModInv(fact[i], mod);
            }
        }

        public static Int64 ModInv(Int64 a, Int64 m)
        {
            Int64 x = 0, y = 0;
            ExtGcd(a, m, ref x, ref y);
            if (x < 0) x += m; //modInv will never be negative
            return x;
        }

        public static Int64 ModPow(Int64 x, Int64 n, Int64 mod)
        {
            Int64 ret = 1;
            while (n > 0)
            {
                if ((n & 1) == 1) ret = ret * x % mod;
                x = x * x % mod;
                n >>= 1;
            }
            return ret;
        }

        public static Int64 Pow(Int64 x, Int64 n)
        {
            Int64 ret = 1;
            while (n > 0)
            {
                if ((n & 1) == 1) ret = ret * x;
                x = x * x;
                n >>= 1;
            }
            return ret;
        }

        //等差数列の和を逆元なしで求める。O(LogN)。
        public static Int64 ModPowSum(Int64 r, Int64 n, Int64 mod)
        {
            if (n == 0) return 0;

            //nが奇数：1 + r + ... + r^(n-1) = 1 + r(1 + r + ... + r^(n-2))
            if (n % 2 == 1) return (ModPowSum(r, n - 1, mod) * r + 1) % mod;

            //nが偶数：1 + r + ... + r^(n-1) = ( 1 + r + ... + r^(n/2-1)) +  r^(n/2) x ( 1 + r + ... + r^(n/2-1))
            Int64 result = ModPowSum(r, n / 2, mod);
            return (result * ModPow(r, n / 2, mod) + result) % mod;
        }

        public static void Sieve(int[] prime, bool[] isPrime)
        {
            for (int i = 0; i < prime.Length; i++) prime[i] = -1;
            for (int i = 0; i < isPrime.Length; i++) isPrime[i] = true;
            isPrime[0] = isPrime[1] = false;

            var idx = 0;
            for (int i = 2; i < isPrime.Length; i++)
            {
                if (isPrime[i])
                {
                    prime[++idx] = i;
                    for (int j = 2 * i; j < isPrime.Length; j += i) isPrime[j] = false;
                }
            }
        }

        // returns primes in [a, b)
        public static void SegSieve(int a, int b, bool[] isPrime)
        {
            for (int i = 0; i < isPrime.Length; i++) isPrime[i] = true;
            for (int i = 0; i < isPrime.Length; i++)
            {
                if (i + a < 2) isPrime[i] = false;
            }

            var sqrtb = (int)Math.Ceiling(Math.Sqrt(b));

            // prime table of [0, sqrt[b])
            var primeTable_0_b = new int[sqrtb];
            var primeTable_0_b_is = new bool[sqrtb];

            Sieve(primeTable_0_b, primeTable_0_b_is);

            foreach (var p in primeTable_0_b)
            {
                if (p == -1) continue;
                var st = (a / p) * p;
                if (st == 0) st = p;
                if (st == p) st += p;
                while (st < b)
                {
                    if (st >= a) isPrime[st - a] = false;
                    st += p;
                }
            }
        }

        public static Int64 ExtGcd(Int64 a, Int64 b, ref Int64 x, ref Int64 y)
        {
            Int64 d = a;
            if (b != 0)
            {
                d = ExtGcd(b, a % b, ref y, ref x);
                y -= (a / b) * x;
            }
            else
            {
                x = 1;
                y = 0;
            }
            return d;
        }

        public static Int64 Gcd(Int64 a, Int64 b)
        {
            if (a < b)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }
            if (b == 0) return a;
            var p = a > b ? a : b;
            return Gcd(b, p % b);
        }

        //素因数分解
        public static Dictionary<UInt64, UInt64> GetFactors(UInt64 n)
        {
            var ret = new Dictionary<UInt64, UInt64>();

            while (n % 2 == 0)
            {
                if (!ret.ContainsKey(2)) ret.Add(2, 0);
                ret[2]++;
                n = n / 2;
            }
            for (UInt64 i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                while (n % i == 0)
                {
                    if (!ret.ContainsKey(i)) ret.Add(i, 0);
                    ret[i]++;
                    n = n / i;
                }
            }
            if (n > 2)
            {
                if (!ret.ContainsKey(n)) ret.Add(n, 0);
                ret[n]++;
            }

            return ret;
        }
    }

    #endregion
}

namespace IO
{
    using System.IO;
    using System.Text;
    using System.Globalization;

    public class Printer : StreamWriter
    {
        static Printer()
        {
            Out = new Printer(Console.OpenStandardOutput()) { AutoFlush = false };
        }

        public static Printer Out { get; set; }

        public override IFormatProvider FormatProvider
        {
            get { return CultureInfo.InvariantCulture; }
        }

        public Printer(Stream stream)
            : base(stream, new UTF8Encoding(false, true))
        {
        }

        public Printer(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
        }

        public void Write<T>(string format, T[] source)
        {
            base.Write(format, source.OfType<object>().ToArray());
        }

        public void WriteLine<T>(string format, T[] source)
        {
            base.WriteLine(format, source.OfType<object>().ToArray());
        }
    }

    public class StreamScanner
    {
        public StreamScanner(Stream stream)
        {
            str = stream;
        }

        public readonly Stream str;
        private readonly byte[] buf = new byte[1024];
        private int len, ptr;
        public bool isEof;

        public bool IsEndOfStream
        {
            get { return isEof; }
        }

        private byte read()
        {
            if (isEof) return 0;
            if (ptr < len) return buf[ptr++];
            ptr = 0;
            if ((len = str.Read(buf, 0, 1024)) > 0) return buf[ptr++];
            isEof = true;
            return 0;
        }

        public char Char()
        {
            byte b;
            do b = read(); while ((b < 33 || 126 < b) && !isEof);
            return (char)b;
        }

        public string Scan()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b >= 33 && b <= 126; b = (char)read())
                sb.Append(b);
            return sb.ToString();
        }

        public string ScanLine()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b != '\n'; b = (char)read())
                if (b == 0) break;
                else if (b != '\r') sb.Append(b);
            return sb.ToString();
        }

        public long Long()
        {
            if (isEof) return long.MinValue;
            long ret = 0;
            byte b;
            var ng = false;
            do b = read(); while (b != 0 && b != '-' && (b < '0' || '9' < b));
            if (b == 0) return long.MinValue;
            if (b == '-')
            {
                ng = true;
                b = read();
            }
            for (; ; b = read())
            {
                if (b < '0' || '9' < b)
                    return ng ? -ret : ret;
                ret = ret * 10 + b - '0';
            }
        }

        public int Integer()
        {
            return (isEof) ? int.MinValue : (int)Long();
        }

        public double Double()
        {
            var s = Scan();
            return s != "" ? double.Parse(s, CultureInfo.InvariantCulture) : double.NaN;
        }

        static T[] enumerate<T>(int n, Func<T> f)
        {
            var a = new T[n];
            for (int i = 0; i < n; ++i) a[i] = f();
            return a;
        }

        public char[] Char(int n)
        {
            return enumerate(n, Char);
        }

        public string[] Scan(int n)
        {
            return enumerate(n, Scan);
        }

        public double[] Double(int n)
        {
            return enumerate(n, Double);
        }

        public int[] Integer(int n)
        {
            return enumerate(n, Integer);
        }

        public long[] Long(int n)
        {
            return enumerate(n, Long);
        }
    }
}

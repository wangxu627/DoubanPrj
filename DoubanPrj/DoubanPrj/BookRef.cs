using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace DoubanPrj
{
    public static class Tuple
    {
        public static Tuple<T1,T2> Create<T1,T2>(T1 item1,T2 item2)
        {
            var n = new Tuple<T1,T2>(item1,item2);
            return n;
        }
    }

    public class Tuple<T1,T2>
    {
        public T1 Item1
        {
            get;set;
        } 
        public T2 Item2
        {
            get;set;
        }

        public Tuple(T1 item1,T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
    

    

    public class CategoryType
    {
        public string Scheme
        {
            get;
            set;
        }

        public string Term
        {
            get;
            set;
        }
    }

    public class RatingType
    {
        public float Average
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public int Min
        {
            get;
            set;
        }

        public int NumRaters
        {
            get;
            set;
        }
    }

    public class BookEntry
    {
        public string Id
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public IList<string> Author    // only name
        {
            get;
            set;
        }
        public CategoryType Category
        {
            get;
            set;
        }
        public Dictionary<string,string> Link
        {
            get;
            set;
        }
        public List<Tuple<string, string>> Attribute
        {
            get;
            set;
        }
        public RatingType Rating
        {
            get;
            set;
        }
    }

    public abstract class XmlData
    {
        protected XmlReader _reader;
        protected string _srchTitle;
        protected int _startIdx;
        protected int _totalCnt;
        protected int _itemsPage;
        
        public XmlData(string filename)
        {
            FileStream fs = new FileStream(filename,FileMode.Open);
            Init(fs);
        }
        
        public XmlData(Stream stream)
        {
            Init(stream);
        }
        
        private void Init(Stream stream)
        {
            _reader = XmlReader.Create(stream);
            while(_reader.Read())
            {
                switch(_reader.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        if(_reader.Name == "title")
                        {
                            _reader.Read();
                            _srchTitle = _reader.Value;
                        }
                        else if(_reader.Name == "opensearch:startIndex")
                        {
                            _reader.Read();
                            _startIdx = Int32.Parse(_reader.Value);
                        }
                        else if(_reader.Name == "opensearch:totalResults")
                        {
                            _reader.Read();
                            _totalCnt = Int32.Parse(_reader.Value);
                        }
                        //else if (_reader.Name == "opensearch:totalResults")
                        //{
                        //    _reader.Read();
                        //    _itemsPage = Int32.Parse(_reader.Value);
                        //}
                        else if(_reader.Name == "entry")
                        {
                            ParseElement();
                            _reader.Read();
                            _itemsPage = Int32.Parse(_reader.Value);
                        }   
                    }
                    break;
                }
            }
        }
       
        protected abstract void ParseElement();
    }

    public class BookXmlData : XmlData
    {
        public string SrchTitle
        {
            get { return _srchTitle; }
        }
        public int StartIdx
        {
            get { return _startIdx; }
        }
        public int TotalCnt
        {
            get { return _totalCnt; }
        }
        public int ItemsPage
        {
            get { return _itemsPage; }
        }
        public IList<BookEntry> Entry
        {
            get { return _entry; }
        }

        private IList<BookEntry> _entry = new List<BookEntry>();

        public BookXmlData(string filename) : base(filename)
        {}
        
        public BookXmlData(Stream stream) : base(stream)
        {}

        protected override void ParseElement()
        {
            bool isClose = false;
            BookEntry be = new BookEntry();
            
            while(_reader.Read())
            {
                if(isClose == true && _reader.Name != "entry")
                {
                    return;
                }
                switch(_reader.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        if(_reader.Name == "entry")
                        {
                            be = new BookEntry();
                            isClose = false;
                        }
                        if(_reader.Name == "id")
                        {
                            _reader.Read();
                            be.Id = _reader.Value;
                            Console.WriteLine("id = " + _reader.Value);
                        }
                        else if(_reader.Name == "title")
                        {
                            _reader.Read();
                            be.Title = _reader.Value;
                            Console.WriteLine("title = " + _reader.Value);
                        }
                        else if(_reader.Name == "category")
                        {
                            //_reader.Read();
                            be.Category = new CategoryType();
                           
                            _reader.MoveToAttribute(0);
                            be.Category.Scheme = _reader.Value;
                            _reader.MoveToAttribute(1);
                            be.Category.Term = _reader.Value;
                        }
                        else if(_reader.Name == "author")
                        {
                            if(be.Author == null)
                            {
                                be.Author = new List<string>();
                            }   
                            _reader.Read();
                            _reader.Read();
                            _reader.Read();
                            be.Author.Add(_reader.Value);
                            _reader.Read();
                            _reader.Read();
                            _reader.Read();
                        }
                        else if(_reader.Name == "link")
                        {
                            if(be.Link == null)
                            {
                                be.Link = new Dictionary<string,string>();
                            }
                            
                            _reader.MoveToAttribute(1);
                            string key = _reader.Value;
                            _reader.MoveToAttribute(0);
                            string val = _reader.Value;
                            
                            be.Link.Add(key,val);
                        }
                        else if(_reader.Name == "db:attribute")
                        {
                            if(be.Attribute == null)
                            {
                                be.Attribute = new List<Tuple<string,string>>();
                            }
                            
                            _reader.MoveToAttribute(0);
                            string item1 = _reader.Value;
                            _reader.Read();
                            string item2 = _reader.Value;
                            
                            be.Attribute.Add(Tuple.Create(item1,item2)); 
                        }
                        else if(_reader.Name == "gd:rating")
                        {
                            be.Rating = new RatingType();
                            _reader.MoveToAttribute(0);
                            be.Rating.Average = float.Parse(_reader.Value);
                            _reader.MoveToAttribute(1);
                            be.Rating.Max = int.Parse(_reader.Value);
                            _reader.MoveToAttribute(2);
                            be.Rating.Min = int.Parse(_reader.Value);
                            _reader.MoveToAttribute(3);
                            be.Rating.NumRaters = int.Parse(_reader.Value);
                        }
                    }
                    break;
                    case XmlNodeType.EndElement:
                    {
                        if(_reader.Name == "entry")
                        {
                            _entry.Add(be);
                            isClose = true;
                            _reader.Read();
                        }
                    }
                    break;
                }
            }
            
        }
    }
}

using System;

namespace TestApp
{
    public class TestRequest : Request<Unit>
    {
        public string Title
        {
            get;
            set;
        }
    }
}
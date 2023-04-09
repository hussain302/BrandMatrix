namespace BrandMatrix.Models.ViewModels
{
    public class Result<T>
    {

        public Result()
        {
            
        }

        private bool _success;
        private string _message;
        private T _data;
		
        public bool Success
		{
			get { return _success; }
			set { _success = value; }
		}

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public T Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}

namespace Cinder.Api.Infrastructure.Dtos
{
    public class ExceptionDto
    {
        public string HelpLink { get; set; }
        public int HResult { get; set; }
        public ExceptionDto InnerException { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}

namespace Owleye.Shared.Base
{
    public interface IAppSession
    {
        public int? Id { get; set; }
        public string EmailAddress { get; set; }
    }
}
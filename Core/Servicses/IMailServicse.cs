namespace Core.Servicses
{
    public interface IMailServicse
    {
        public Task sendEmail(string mailTo, string Subject, string body);
    }
}

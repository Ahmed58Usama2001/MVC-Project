using Demo.DAL.Models;

namespace Demo.PL.Helpers
{
    public interface IMailSetting
    {
        public void SendEmail(Email email);
    }
}

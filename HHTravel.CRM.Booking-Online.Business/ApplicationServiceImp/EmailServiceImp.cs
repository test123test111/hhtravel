using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.IRepository;

namespace HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EmailServiceImp : IEmailService
    {
        private IEmailRepository _repoEmail;

        public IEmailRepository EmailRepository
        {
            get
            {
                if (_repoEmail == null)
                    _repoEmail = RepositoryFactory.GetRepository<IEmailRepository>();
                return _repoEmail;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        public void Send(Model.Email email)
        {
            this.EmailRepository.Insert(email);
        }
    }
}

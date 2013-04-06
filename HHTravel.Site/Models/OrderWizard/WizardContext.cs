using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using HHTravel.CRM.Booking_Online.DataService;

namespace HHTravel.CRM.Booking_Online.Site.Models.OrderWizard
{
    public class WizardContext
    {
        private static ITempOrderProvider s_tempOrderProvider = new SqlTempOrderProvider();

        // 创建成本较高，缓存之
        public List<FlightsSegmentModel> FlightsSegmentModels { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }
        public Step1PostModel Step1Model { get; set; }
        public Step2PostModel Step2Model { get; set; }
        public Step3PostModel Step3Model { get; set; }
        public Step4PostModel Step4Model { get; set; }
        public Step5PostModel Step5Model { get; set; }


        public static WizardContext Read(Guid sessionId)
        {
            WizardContext context;
            string json = s_tempOrderProvider.GetContent(sessionId);
            if (string.IsNullOrEmpty(json))
            {
                context = new WizardContext();
            }
            else
            {
                context = JsonConvert.DeserializeObject<WizardContext>(json);
            }


            //context = JsonConvert.DeserializeObject<WizardContext>(json, new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.All,
            //    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            //});
            return context;
        }

        public int GetBeginSubstepNo(int stepNo)
        {
            int beginSubstepNo;
            int preStepNo = stepNo - 1;
            beginSubstepNo = GetEndSubstepNo(preStepNo) + 1;
            return beginSubstepNo;
        }

        public void Write(Guid sessionId)
        {
            string json = JsonConvert.SerializeObject(this);
            s_tempOrderProvider.AddOrUpdate(sessionId, json);
        }

        // !HARDCODE !RECURSIVE
        private int GetEndSubstepNo(int stepNo)
        {
            switch (stepNo)
            {
                case 1:
                    return 3;
                case 2:
                    int endSubstepNo = GetEndSubstepNo(1) + 4;// 小于或等于4个子步骤
                    if (Step2Model.DelayHotelPostModel == null)
                    {
                        endSubstepNo--;
                    }
                    if (Step2Model.SelectedGroundServiceId == 0)
                    {
                        endSubstepNo--;
                    }

                    return endSubstepNo;    
                case 3:
                    return GetEndSubstepNo(2) + (Step2Model.IsChooseFlights ? 1 : 0); // 1或0个子步骤
                case 4:
                    return GetEndSubstepNo(3) + 3;  // 3个子步骤
                default:
                    throw new ArgumentOutOfRangeException("stepNo", "其他步骤暂不需要此服务");
            };
        }
    }
}
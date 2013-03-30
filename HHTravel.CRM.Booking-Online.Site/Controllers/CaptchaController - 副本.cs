using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Helpers;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class CaptchaController : Controller
    {
        //
        // GET: /Captcha/

        public string New()
        {
            string newid = Guid.NewGuid().ToString();
            string value = RndNum(6);
            Session["Captcha"] = new KeyValuePair<string, string>(newid, value);

            return newid;
        }

        public void Index(string id)
        {
            var captchar = (KeyValuePair<string, string>)Session["Captcha"];

            if (captchar.Key == id)
            {
                CreateImage(captchar.Value);
            }
        }

        [HttpPost]
        public ActionResult Validate(string guid, string code)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(guid) && !string.IsNullOrEmpty(code))
            {
                var captchar = (KeyValuePair<string, string>)Session["Captcha"];
                result = (guid != captchar.Key || code != captchar.Value) ? 0 : 1;
            }

            return Json(result);
        }

        private string RndNum(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p" +
             ",q,r,s,t,u,v,w,x,y,z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            //int temp = -1;
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                //if (temp != -1)
                //{
                //    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                //}
                int t = rand.Next(36);
                //if (temp != -1 && temp == t)
                //{
                //    return RndNum(VcodeNum);
                //}
                //temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }

        //生成随机颜色
        private Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        public void CreateImage(string str_ValidateCode)
        {
            int int_ImageWidth = str_ValidateCode.Length * 13;
            Random newRandom = new Random();
            //  图高20px
            Bitmap theBitmap = new Bitmap(int_ImageWidth, 20);
            Graphics theGraphics = Graphics.FromImage(theBitmap);
            //  白色背景
            theGraphics.Clear(Color.White);
            //  灰色边框
            theGraphics.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, 19);
            //  10pt的字体
            Font theFont = new Font("Arial", 10);
            for (int int_index = 0; int_index < str_ValidateCode.Length; int_index++)
            {
                string str_char = str_ValidateCode.Substring(int_index, 1);
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(int_index * 13 + 1 + newRandom.Next(3), 1 + newRandom.Next(3));
                theGraphics.DrawString(str_char, theFont, newBrush, thePos);
            }
            //  将生成的图片发回客户端
            MemoryStream ms = new MemoryStream();
            theBitmap.Save(ms, ImageFormat.Jpeg);

            new WebImage(ms.ToArray()).Write();

            theGraphics.Dispose();
            theBitmap.Dispose();
        }
    }
}

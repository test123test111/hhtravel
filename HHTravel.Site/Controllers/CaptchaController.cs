using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
//using HHTravel.Base.Common.Framework.Security;

namespace HHTravel.Site.Controllers
{
    public class CaptchaController : Controller
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public static bool Validate(string captcha)
        {
            bool result = false;
            var stored = CookieManager.Captcha as string;
            if (!string.IsNullOrEmpty(stored) && !string.IsNullOrEmpty(captcha) && captcha.Length == 6)
            {
                string random = string.Empty;
                if (Decrypt(stored, out random))
                {
                    result = string.Equals(random, captcha, StringComparison.OrdinalIgnoreCase);
                }
            }
            CookieManager.Captcha = null;
            return result;
        }

        public void Index()
        {
            string randomText = CreateValidateNumber(6);
            CookieManager.Captcha = Encrypt(randomText);
            CreateValidateGraphic(randomText, 100, 30);
        }

        /// <summary>
        /// 创建验证码的图片
        /// 2012-11-21，刘剡，简化验证码:单一颜色，减少干扰线，字体全粗，加大字体间距
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        protected static void CreateValidateGraphic(string validateNum, int width, int height)
        {
            //获取字号PX值
            float fontSizePX = GetFontSizePX(width, height, validateNum.Length);

            //换算成PT值
            int fontSizePT = Convert.ToInt32(Math.Ceiling(fontSizePX * 3 / 4));

            //文字宽度
            float validateNumWidth = fontSizePX * validateNum.Length / 2;

            //第一个字符左边的位置
            float leftPositionOfFirstChar = width / 2 - validateNumWidth / 2 - fontSizePX / 4 - 5;

            //生成随机生成器
            Random random = new Random();
            //设定笔触
            int[] webColorPool = { 0, 51, 102, 153, 204 };

            //设定字体颜色
            //Color fontColor = Color.FromArgb(webColorPool[random.Next(webColorPool.Length)], webColorPool[random.Next(webColorPool.Length)], webColorPool[random.Next(webColorPool.Length)]);
            Color fontColor = Color.Blue;

            using (Bitmap image = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(image))
            {
                //清空图片背景色
                g.Clear(Color.White);

                //画图片的干扰线
                for (int i = 0; i < fontSizePX / 5; i++)
                {
                    Point lineStartPoint = new Point(random.Next(width), random.Next(height));
                    Point lineEndPoint = new Point(random.Next(width), random.Next(height));

                    using (Pen linePen = new Pen(Color.FromArgb(random.Next()), Convert.ToSingle(fontSizePX * 0.025)))
                    {
                        g.DrawLine(linePen, lineStartPoint, lineEndPoint);
                    }
                }

                //设定笔触
                //using(LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, width, height), Color.Blue, Color.DarkRed, 1.2f, true))

                for (int charIndex = 0; charIndex < validateNum.Length; charIndex++)
                {
                    //设定左上角点
                    PointF topLeftPoint = new PointF();
                    topLeftPoint.X = leftPositionOfFirstChar + fontSizePX * charIndex * 2 / 3;

                    //随机漂移
                    //topLeftPoint.X += random.Next(Convert.ToInt32(-1 * fontSizePX / 6), Convert.ToInt32(fontSizePX / 6));

                    if (height > fontSizePX * 2.5)
                    {
                        topLeftPoint.Y = random.Next(Convert.ToInt32(height / 2 - fontSizePX), Convert.ToInt32(height / 2));
                    }
                    else
                    {
                        topLeftPoint.Y = random.Next(Convert.ToInt32(0.25 * fontSizePX), Convert.ToInt32(height - 1.25 * fontSizePX));
                    }

                    //设定字体
                    FontStyle fStyle = FontStyle.Regular;
                    switch (random.Next(4))
                    {
                        case 0:

                            //fStyle = FontStyle.Regular;
                            fStyle = FontStyle.Bold;
                            break;

                        case 1:
                            fStyle = FontStyle.Bold;
                            break;

                        case 2:

                            //fStyle = FontStyle.Italic;
                            fStyle = FontStyle.Bold;
                            break;

                        case 3:

                            //fStyle = FontStyle.Bold | FontStyle.Italic;
                            fStyle = FontStyle.Bold;
                            break;
                    }
                    using (Font charFont = new Font("Courier New", fontSizePT, fStyle))
                    using (LinearGradientBrush fontBrush = new LinearGradientBrush(new Rectangle(Convert.ToInt32(topLeftPoint.X), Convert.ToInt32(topLeftPoint.Y), Convert.ToInt32(topLeftPoint.X + 0.75 * fontSizePX), Convert.ToInt32(topLeftPoint.Y + fontSizePX)), fontColor, fontColor, 1.2f, true))
                    {
                        g.DrawString(validateNum.Substring(charIndex, 1), charFont, fontBrush, topLeftPoint);
                    }
                }

                //画图片的前景干扰点
                for (int i = 0; i < Math.Sqrt(width * height); i++)
                {
                    int x = random.Next(width);
                    int y = random.Next(height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                using (var pen = new Pen(Color.Silver))
                {
                    g.DrawRectangle(pen, 0, 0, width - 1, height - 1);
                }

                //保存图片数据
                using (MemoryStream stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Jpeg);
                    new WebImage(stream.ToArray()).Write();
                }
            }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>随机生成的验证码字符串</returns>
        protected static string CreateValidateNumber(int length)
        {
            string[] chars = { "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Q", "R", "T", "a", "b", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "q", "r", "t" };

            StringBuilder validateNumber = new StringBuilder();
            Random myRandom = new Random();

            while (validateNumber.Length < length)
            {
                int index = myRandom.Next(chars.Length);

                validateNumber.Append(chars[index]);
            }

            return validateNumber.ToString();
        }

        /// <summary>
        /// 解密验证码
        /// </summary>
        /// <param name="cookieValue">cookie明文</param>
        /// <param name="captcha">解密后验证码</param>
        /// <returns>cookie是否安全</returns>
        private static bool Decrypt(string cookieValue, out string captcha)
        {
            //captcha = string.Empty;
            //string base64 = SecurityHelper.DecodeBase64(cookieValue);
            //string[] cookieValueArr = base64.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            //string md5 = SecurityHelper.MD5Encrypt(cookieValueArr[0]);
            //if (cookieValueArr[1] == md5)
            //{
            //    string decryptText = SecurityHelper.DesDecrypt(cookieValueArr[0]);
            //    string[] decryptTextArr = decryptText.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            //    DateTime dt = DateTime.Parse(decryptTextArr[1]);
            //    TimeSpan ts = DateTime.Now - dt;

            //    //超时20分钟
            //    if (ts.TotalMinutes <= 20)
            //    {
            //        captcha = decryptTextArr[0];
            //        return true;
            //    }
            //}
            //return false;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加密验证码
        /// </summary>
        /// <param name="captcha">验证码</param>
        /// <returns>加密后的字符串</returns>
        private static string Encrypt(string captcha)
        {
            //string encryptPlain = string.Format("{0}|{1}", captcha, DateTime.Now);
            //string encryptText = SecurityHelper.DesEncrypt(encryptPlain);
            //string md5 = SecurityHelper.MD5Encrypt(encryptText);
            //return SecurityHelper.EncodeBase64(string.Format("{0}|{1}", encryptText, md5));
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取字体的字号的PX值
        /// </summary>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="validationNumberLength">验证码长度</param>
        /// <returns>字号的PX值</returns>
        private static float GetFontSizePX(int width, int height, int validationNumberLength)
        {
            //从横向上计算字号
            float fontSizePX = Convert.ToSingle(width / (validationNumberLength / 2 + 0.5));

            //如果字号超过高度
            if ((fontSizePX + 0.5 * fontSizePX) > height)
            {
                //从高度上计算字号
                fontSizePX = Convert.ToSingle(height / 1.5);
            }

            return fontSizePX;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web.Helpers;
using System.Drawing.Imaging;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class VerifyCodeController : Controller
    {
        public void Index()
        {
            string randomText = CreateValidateNumber(6);
            Session["VerifyCode"] = randomText;

            CreateValidateGraphic(randomText, 100, 30);
        }
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>随机生成的验证码字符串</returns>       
        public static string CreateValidateNumber(int length)
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
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>     
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void CreateValidateGraphic(string validateNum, int width, int height)
        {
            Bitmap image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);

            //获取字号PX值
            float fontSizePX = GetFontSizePX(width, height, validateNum.Length);
            //换算成PT值
            int fontSizePT = Convert.ToInt32(Math.Ceiling(fontSizePX * 3 / 4));

            //文字宽度
            float validateNumWidth = fontSizePX * validateNum.Length / 2;

            //第一个字符左边的位置
            float leftPositionOfFirstChar = width / 2 - validateNumWidth / 2 - fontSizePX / 4;


            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的干扰线
                for (int i = 0; i < fontSizePX / 2; i++)
                {
                    Pen linePen = new Pen(Color.FromArgb(random.Next()), Convert.ToSingle(fontSizePX * 0.025));
                    Point lineStartPoint = new Point(random.Next(image.Width), random.Next(image.Height));
                    Point lineEndPoint = new Point(random.Next(image.Width), random.Next(image.Height));

                    g.DrawLine(linePen, lineStartPoint, lineEndPoint);
                }

                //设定笔触
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);

                //设定笔触
                int[] webColorPool = { 0, 51, 102, 153, 204 };

                //设定字体颜色
                Color fontColor = Color.FromArgb(webColorPool[random.Next(webColorPool.Length)], webColorPool[random.Next(webColorPool.Length)], webColorPool[random.Next(webColorPool.Length)]);

                for (int charIndex = 0; charIndex < validateNum.Length; charIndex++)
                {
                    //设定左上角点
                    PointF topLeftPoint = new PointF();
                    topLeftPoint.X = leftPositionOfFirstChar + fontSizePX * charIndex / 2;
                    //随机漂移
                    topLeftPoint.X += random.Next(Convert.ToInt32(-1 * fontSizePX / 6), Convert.ToInt32(fontSizePX / 6));

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
                            fStyle = FontStyle.Regular;
                            break;
                        case 1:
                            fStyle = FontStyle.Bold;
                            break;
                        case 2:
                            fStyle = FontStyle.Italic;
                            break;
                        case 3:
                            fStyle = FontStyle.Bold | FontStyle.Italic;
                            break;
                    }
                    Font charFont = new Font("Courier New", fontSizePT, fStyle);

                    LinearGradientBrush fontBrush = new LinearGradientBrush(new Rectangle(Convert.ToInt32(topLeftPoint.X), Convert.ToInt32(topLeftPoint.Y), Convert.ToInt32(topLeftPoint.X + 0.75 * fontSizePX), Convert.ToInt32(topLeftPoint.Y + fontSizePX)), fontColor, fontColor, 1.2f, true);

                    g.DrawString(validateNum.Substring(charIndex, 1), charFont, fontBrush, topLeftPoint);
                }



                //画图片的前景干扰点
                for (int i = 0; i < Math.Sqrt(width * height); i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                new WebImage(stream.ToArray()).Write();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
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

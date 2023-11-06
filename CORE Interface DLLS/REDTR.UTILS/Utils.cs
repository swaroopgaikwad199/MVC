using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Windows.Forms;

namespace REDTR.UTILS
{
    public class Utils
    {
        public static long GetTimeDiff_MS(Stopwatch sw)
        {
            long milliseconds = (long)((double)sw.ElapsedTicks / ((double)Stopwatch.Frequency / (double)(1000L)));
            return milliseconds;
        }
        public static long GetTimeDiff_US(Stopwatch sw)
        {
            long microseconds = (long)((double)sw.ElapsedTicks / ((double)Stopwatch.Frequency / (double)(1000L * 1000L)));
            return microseconds;
        }
        public static long GetTimeDiff_NS(Stopwatch sw)
        {
            long nanoseconds = (long)((double)sw.ElapsedTicks / ((double)Stopwatch.Frequency / (double)(1000L * 1000L * 1000L)));
            return nanoseconds;
        }

        public static bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }
        public static void ProcessStart(string name)
        {
            Process proc2Start = new Process();

            proc2Start.StartInfo.FileName = name;
            //proc2Start.StartInfo.Arguments = "ProcessStart.cs";

            proc2Start.Start();
        }
        public static void ProcessEnd(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    clsProcess.Kill();
                }
            }
        }
        public static bool RunningInstanceSelf()
        {
            bool Retval = false;
            string currentProc = Assembly.GetCallingAssembly().FullName.Split(',')[0];
            Process[] processes = Process.GetProcessesByName(currentProc);
            foreach (Process item in processes)
            {
                if (string.Compare(item.ProcessName, currentProc, false) == 0)
                {
                    if (Retval)
                        return true;
                    Retval = true;
                }
            }
            return false;
        }
        public static bool RunningInstanceSelf(string assemblytostart)
        {
            bool Retval = false; 
            Process[] processes = Process.GetProcessesByName(assemblytostart);
            foreach (Process item in processes)
            {
                if (string.Compare(item.ProcessName, assemblytostart, false) == 0)
                {
                    if (Retval)
                        return true;
                    Retval = true;
                }
            }
            return false;
        }
        public static bool RunningInstance(string CurrentProc)
        {
            string Path = string.Empty;
            bool Retval = false;
            Process[] processes = Process.GetProcessesByName(CurrentProc);
            //Loop through the running processes in with the same name 
            foreach (Process item in processes)
            {                
                //if (item.ProcessName.CompareTo(CurrentProc) == 0)
                if(string.Compare(item.ProcessName, CurrentProc, false) == 0)
                {
                    Retval = true;
                    break;
                }
            }
            return Retval;
        }

        public static void CursorWaitSet(Form ob)
        {
            //System.Windows.Forms.Cursor CurCursor = ob.Cursor;
            ob.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            ob.UseWaitCursor = true;
        }
       
        public static void CursorReset(Form ob)
        {
            ob.Cursor = System.Windows.Forms.Cursors.Default;
            ob.UseWaitCursor = false;
        }

        //        #region IMAGE_DUMP
        //        private string imageDumpPath = "d:\\TnT_Images\\";
        //        private bool isDumpImage = true;
        //        public enum DumpType
        //        {
        //            Ref,
        //            Run,
        //            Teach,
        //            Test
        //        }
        //        private void DumpImage(System.Drawing.Bitmap image, DumpType dumpAs)
        //        {
        //            if (isDumpImage == true)
        //            {
        //                try
        //                {
        //                    string name = "TnT_";
        //                    //name += jobMaster.JobDbId.ToString() + "_" + jobResult.Total.ToString().PadLeft(4, '0');
        //                    name += ".bmp";
        //                    image.Save(imageDumpPath + name, System.Drawing.Imaging.ImageFormat.Bmp);
        //                }
        //                catch
        //                { }
        //            }
        //        }
        //        private void DumpImage(System.Drawing.Bitmap image, DumpType dumpAs, bool GoodJob)
        //        {
        //            if (isDumpImage == true && GoodJob == false)
        //            {
        //                try
        //                {
        //                    string name = "TnT_" + dumpAs.ToString() + (GoodJob == true ? "Good_" : "Bad_");
        //                    //name += jobMaster.JobDbId.ToString() + "_" + jobResult.Total.ToString().PadLeft(4, '0');
        //                    name += ".bmp";
        //                    image.Save(imageDumpPath + name, System.Drawing.Imaging.ImageFormat.Bmp);
        //                }
        //                catch
        //                { }
        //            }
        //        }
        //        #endregion IMAGE_DUMP

        ////#if !MONO
        //        // Win32 memory copy function
        //        [DllImport("ntdll.dll")]
        //        private static unsafe extern byte* memcpy(
        //            byte* dst,
        //            byte* src,
        //            int count);
        //#endif
        //        [DllImport(@"RIMBASE.dll", SetLastError = true)]
        //        unsafe static extern string DecodeFileName(string name);

        //        public static string DecodeCharFileName(string oName)
        //        {
        //            return DecodeFileName(oName);
        //        }
        //        public static byte[] BitmapToByteArray(Bitmap b, out UInt32 stride, out int width, out int height)
        //        {
        //            Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);
        //            BitmapData bd = b.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        //            width = bd.Width;
        //            height = bd.Height;
        //            try
        //            {
        //                byte[] pxl = new byte[bd.Stride * b.Height];
        //                Marshal.Copy(bd.Scan0, pxl, 0, bd.Stride * b.Height);
        //                stride = (UInt32)bd.Stride;
        //                return pxl;
        //            }
        //            finally
        //            {
        //                b.UnlockBits(bd);
        //            }
        //        }
        //        public static byte[] BitmapToByteArrayEx(Bitmap b, out UInt32 stride, out int width, out int height)
        //        {
        //            Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);
        //            BitmapData bd = b.LockBits(rect, ImageLockMode.ReadOnly, b.PixelFormat);
        //            width = bd.Width;
        //            height = bd.Height;
        //            try
        //            {
        //                byte[] pxl = new byte[bd.Stride * b.Height];
        //                Marshal.Copy(bd.Scan0, pxl, 0, bd.Stride * b.Height);
        //                stride = (UInt32)bd.Stride;
        //                return pxl;
        //            }
        //            finally
        //            {
        //                b.UnlockBits(bd);
        //            }
        //        }
        //        public static unsafe Bitmap ByteArrayToBitmap(int Width, int Height, byte* b)
        //        {
        //            Rectangle rect = new Rectangle(0, 0, Width, Height);
        //            Bitmap TEMP = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed);
        //            BitmapData bd = TEMP.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
        //            SetGrayscalePalette(TEMP);
        //            try
        //            {
        //                byte* tdst = (byte*)bd.Scan0.ToPointer();
        //                for (int i = 0; i < bd.Stride * Height; i++)
        //                    tdst[i] = b[i];
        //                //Marshal.Copy(b, 0, bd.Scan0, bd.Stride * Height);
        //                //byte[] pxl = new byte[bd.Stride * b.Height];
        //                //Marshal.Copy(b, bd.Scan0, 0, bd.Stride * Height);
        //            }
        //            catch (Exception ex)
        //            {
        //                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
        //            }
        //            finally
        //            {
        //                TEMP.UnlockBits(bd);
        //            }
        //            return TEMP;
        //        }
        //        public static void SetGrayscalePalette(Bitmap image)
        //        {
        //            // check pixel format
        //            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
        //                throw new ArgumentException("Source image is not 8 bpp image.");

        //            // get palette
        //            ColorPalette cp = image.Palette;
        //            // init palette
        //            for (int i = 0; i < 256; i++)
        //            {
        //                cp.Entries[i] = Color.FromArgb(i, i, i);
        //            }
        //            // set palette back
        //            image.Palette = cp;
        //        }
        //        public static Bitmap ConvertRGB24ToMonochrome(Bitmap source)
        //        {
        //            int stride;
        //            if ( source.PixelFormat != PixelFormat.Format24bppRgb )
        //            {
        //                source = source.Clone(new Rectangle(0, 0, source.Width, source.Height),PixelFormat.Format24bppRgb);
        //            }


        //            Byte[] colorBytes = BitmapToBytesRGB24(source,out stride);

        //            if ( colorBytes == null )
        //            {
        //                 return null;
        //            }

        //            Byte[] monoBytes = new Byte[source.Width * source.Height];

        //            int j = 0;

        //            for ( int i = 0; i < source.Width * source.Height; i++ )
        //            {
        //                monoBytes[i] = (Byte)(0.3f * (float)colorBytes[j + 2] + 0.59f * (float)colorBytes[j + 1] + 0.11f * (float)colorBytes[j]);

        //                j += 3;
        //            }

        //            return BitsToBitmapMonochrome(monoBytes, source.Width, source.Height);
        //        }
        //        public static Bitmap BitsToBitmapMonochrome(Byte[] bytes, int width, int height)
        //        {
        //            if (bytes.GetLength(0) < width * height)
        //            {
        //                return null;
        //            }

        //            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
        //            int i;

        //            bmp.Palette = GetGrayScalePalette();

        //            //for (i = 0; i < 256; i++)
        //            //{
        //            //    bmp.Palette.Entries[i] = Color.FromArgb(i, i, i);
        //            //}


        //            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
        //                ImageLockMode.WriteOnly, bmp.PixelFormat);


        //            if (data.Stride == width * 3)
        //            {
        //                Marshal.Copy(bytes, 0, data.Scan0, width * height);
        //            }
        //            else
        //            {
        //                for (i = 0; i < bmp.Height; i++)
        //                {
        //                    IntPtr p = new IntPtr(data.Scan0.ToInt32() + data.Stride * i);
        //                    Marshal.Copy(bytes, i * bmp.Width, p, bmp.Width);
        //                }
        //            }

        //            bmp.UnlockBits(data);

        //            return bmp;
        //        }
        //        public static Byte[] BitmapToBytesRGB24(Bitmap bmp,out int stride)
        //        {
        //            stride = 0;

        //            if ( bmp.PixelFormat != PixelFormat.Format24bppRgb )
        //            {
        //                return null;
        //            }

        //            int i;
        //            int length = bmp.Width * bmp.Height * 3;

        //            Byte[] bytes = new Byte[length];

        //            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
        //                ImageLockMode.ReadOnly, bmp.PixelFormat);

        //            stride = Math.Abs(data.Stride);

        //            if (data.Stride == bmp.Width * 3) 
        //            {
        //                Marshal.Copy(data.Scan0, bytes, 0, length);
        //            }
        //            else
        //            {
        //                for ( i = 0; i < bmp.Height; i++ )
        //                {
        //                    IntPtr p = new IntPtr(data.Scan0.ToInt32() + data.Stride * i);
        //                    Marshal.Copy(p, bytes, i * bmp.Width * 3, bmp.Width * 3);
        //                }
        //            }

        //            bmp.UnlockBits(data);

        //            return bytes;
        //        }
        //        public static unsafe Bitmap BitsToBitmapRGB24(byte *strm, int width, int height)
        //        {
        //            int i;
        //            long length = width* height * 3;
        //            byte[] bytes = new byte[length];
        //            for ( i = 0; i < length; i++)
        //                bytes[i] = Marshal.ReadByte((IntPtr)strm, i);

        //            if (bytes.GetLength(0) < width * height * 3)
        //            {
        //                return null;
        //            }

        //            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

        //            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
        //                ImageLockMode.WriteOnly, bmp.PixelFormat);


        //            if (data.Stride == width * 3)
        //            {
        //                Marshal.Copy(bytes, 0, data.Scan0, width * height * 3);
        //            }
        //            else
        //            {
        //                for (i = 0; i < bmp.Height; i++)
        //                {
        //                    IntPtr p = new IntPtr(data.Scan0.ToInt32() + data.Stride * i);
        //                    Marshal.Copy(bytes, i * bmp.Width * 3, p, bmp.Width * 3);
        //                }
        //            }

        //            bmp.UnlockBits(data);

        //            return bmp;
        //        }
        //        public static Bitmap rotateImage(Bitmap b, float angle)
        //        {
        //            //create a new empty bitmap to hold rotated image
        //            Bitmap returnBitmap = new Bitmap(b.Height, b.Width);
        //            //make a graphics object from the empty bitmap
        //            Graphics g = Graphics.FromImage(returnBitmap);
        //            //move rotation point to center of image
        //            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
        //            //rotate
        //            g.RotateTransform(angle);
        //            //move image back
        //            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
        //            //draw passed in image onto graphics object
        //            g.DrawImage(b, new Point(0, 0));
        //            return returnBitmap;
        //        }
        //        public static System.Drawing.Imaging.ColorPalette GetGrayScalePalette()
        //        {
        //            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

        //            ColorPalette monoPalette = bmp.Palette;

        //            Color[] entries = monoPalette.Entries;

        //            for ( int i = 0; i < 256; i++ )
        //            {
        //                entries[i] = Color.FromArgb(i, i, i);
        //            }

        //            return monoPalette;
        //        }
        //        public static string GetPharmaCodeFromBars(string seq, bool ReadReverse)
        //        {
        //            int num = 0; ;
        //            if (seq != null && seq != string.Empty)
        //            {
        //                char[] x = seq.ToCharArray();

        //                if (ReadReverse == true)
        //                {
        //                    for (int i = 0, j = seq.Length - 1; i <= seq.Length / 2; i++, j--)
        //                    {
        //                        char tmp = x[i];
        //                        x[i] = x[j];
        //                        x[j] = tmp;
        //                    }
        //                }
        //                for (int i = 0; i < seq.Length; i++)
        //                {
        //                    if (((int)x[i] == '1'))
        //                        num += (int)Math.Pow((double)2, (double)i) * 2;
        //                    else
        //                        num += (int)Math.Pow((double)2, (double)i);
        //                }
        //            }
        //            return num.ToString();
        //        }
        //        public static string GetBarSequenceFromCode(string PharmaCode)
        //        {
        //            double barcode = int.Parse(PharmaCode);
        //            string str = "";
        //            while (barcode != 0)
        //            {
        //                if (barcode % 2 == 1)
        //                {
        //                    str += "0";
        //                    barcode = (barcode - 1) / 2;
        //                }
        //                else
        //                {
        //                    str += "1";
        //                    barcode = (barcode - 2) / 2;
        //                }
        //            }

        //            return str;
        //        }
        //        public static byte[] Bitmap8bppToByteArray(Bitmap b, out UInt32 m_stride, out int m_width, out int m_height)
        //        {
        //            byte []Dst = null;
        //            m_stride = 0;
        //            m_width = m_height = 0;
        //            BitmapData m_BitmapData = b.LockBits(new Rectangle(0, 0, b.Size.Width, b.Size.Height), ImageLockMode.ReadWrite, b.PixelFormat);
        //            try
        //            {
        //                m_width = m_BitmapData.Width;
        //                m_height = m_BitmapData.Height;
        //                m_stride = (uint)m_BitmapData.Stride;
        //                Dst = new byte[m_width * m_height];
        //                int srcOffset = m_BitmapData.Stride - ((m_BitmapData.PixelFormat == PixelFormat.Format8bppIndexed) ? m_width : m_width * 3);
        //                unsafe
        //                {
        //                    byte* src = (byte*)m_BitmapData.Scan0.ToPointer();
        //                    if (m_BitmapData.PixelFormat == PixelFormat.Format8bppIndexed)
        //                    {
        //                        for (int y = 0; y < m_height; y++)
        //                        {
        //                            for (int x = 0; x < m_width; x++, src++)
        //                            {
        //                                Dst[y * m_width + x] = *src;
        //                            }
        //                            src += srcOffset;
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
        //            }

        //            finally
        //            {
        //                b.UnlockBits(m_BitmapData);
        //            }
        //            return Dst;
        //        }
        //        public static unsafe byte* CopyUnmanagedMemory(byte* dst, byte* src, int count)
        //        {
        //#if !MONO
        //            return memcpy(dst, src, count);
        //#else
        //            int countUint = count >> 2;
        //            int countByte = count & 3;

        //            uint* dstUint = (uint*) dst;
        //            uint* srcUint = (uint*) src;

        //            while ( countUint-- != 0 )
        //            {
        //                *dstUint++ = *srcUint++;
        //            }

        //            byte* dstByte = (byte*) dstUint;
        //            byte* srcByte = (byte*) srcUint;

        //            while ( countByte-- != 0 )
        //            {
        //                *dstByte++ = *srcByte++;
        //            }
        //            return dst;
        //#endif
        //        }
        //        public static IntPtr CopyUnmanagedMemory(IntPtr dst, IntPtr src, int count)
        //        {
        //            unsafe
        //            {
        //                CopyUnmanagedMemory((byte*)dst.ToPointer(), (byte*)src.ToPointer(), count);
        //            }
        //            return dst;
        //        }
        //        public static Bitmap Clone24BitBitmap(Bitmap source)
        //        {
        //            Bitmap dest = null;
        //            if (source != null)
        //            {
        //                dest = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);
        //                BitmapData datadest = dest.LockBits(new Rectangle(0,0,source.Width, source.Height),ImageLockMode.ReadWrite,source.PixelFormat);
        //                BitmapData datasrc = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly,source.PixelFormat);
        //                if (datasrc != null && datadest != null)
        //                {
        //                    CopyUnmanagedMemory(datadest.Scan0, datasrc.Scan0, Math.Abs(datasrc.Stride) * datasrc.Height);
        //                    dest.UnlockBits(datadest);
        //                    source.UnlockBits(datasrc);             
        //                }

        //            }
        //            return dest;
        //        }
        //        public static unsafe void Copy(byte* src, byte[] dst, int count)
        //        {
        //            if (src == null ||dst == null || count < 0)
        //            {
        //                throw new System.ArgumentException();
        //            }

        //            int srcLen = count;
        //            int dstLen = dst.Length;

        //            // The following fixed statement pins the location of the src and dst objects
        //            // in memory so that they will not be moved by garbage collection.
        //            fixed (byte* pDst = dst)
        //            {
        //                byte* ps = src;
        //                byte* pd = pDst;

        //                // Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
        //                for (int i = 0; i < count / 4; i++)
        //                {
        //                    *((int*)pd) = *((int*)ps);
        //                    pd += 4;
        //                    ps += 4;
        //                }

        //                // Complete the copy by moving any bytes that weren't moved in blocks of 4:
        //                for (int i = 0; i < count % 4; i++)
        //                {
        //                    *pd = *ps;
        //                    pd++;
        //                    ps++;
        //                }
        //            }
        //        }
        //        public static Rectangle RectFToRect(RectangleF rect)
        //        {
        //            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        //        }
        //        public static void SetRoatation(Bitmap bmp, int Angle)
        //        {
        //            switch (Angle)
        //            {
        //                case 0:
        //                    break;
        //                case 90:
        //                    bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //                    break;
        //                case 180:
        //                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //                    break;
        //                case 270:
        //                    bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //                    break;
        //            }
        //        }
        //        public static void RoatatePoint(int x, int y, ref int x1, ref int y1, double anglerad)
        //        {
        //            x1 = (int)(((double)x) * Math.Cos(anglerad) - ((double)y) * Math.Sin(anglerad));
        //            y1 = (int)(((double)x) * Math.Sin(anglerad) + ((double)y) * Math.Cos(anglerad));
        //        }
        //        public static Point[] RotateRect(int W,int H, Point[] pointsList, double anglerad)
        //        {
        //            int numPoints = 4;
        //            Point[] pnt = new Point[4];

        //            for (int currPoint = 0; currPoint < numPoints; currPoint++)
        //            {
        //                Point point = pointsList[currPoint];

        //                int x, y, x1 =0, y1 = 0;
        //                x = point.X - W / 2;
        //                y = point.Y - H / 2;

        //                RoatatePoint(x, y, ref x1, ref y1, -anglerad);

        //                point.X = x1 + W / 2;
        //                point.Y = y1 + H / 2;

        //                pnt[currPoint].X = point.X;
        //                pnt[currPoint].Y = point.Y;
        //            }
        //            return pnt;
        //        }
        //        public static Point[] RectToPoints(Rectangle rect)
        //        {
        //            Point[] pts = new Point[4];
        //            pts[0].X = rect.Left;
        //            pts[0].Y = rect.Top;
        //            pts[1].X = rect.Right;
        //            pts[1].Y = rect.Top;
        //            pts[2].X = rect.Right;
        //            pts[2].Y = rect.Bottom;
        //            pts[3].X = rect.Left;
        //            pts[3].Y = rect.Bottom;

        //            return pts;
        //        }
        //        public static Rectangle PointsToRects(Point[] pts)
        //        {
        //            Rectangle rect = new Rectangle();
        //            int minx,miny,maxx,maxy;
        //            minx = pts[0].X;
        //            foreach (Point pt in pts)
        //                if (minx > pt.X)
        //                    minx = pt.X;

        //            miny = pts[0].Y;
        //            foreach (Point pt in pts)
        //                if (miny > pt.Y)
        //                    miny = pt.Y;

        //            maxx = pts[0].X;
        //            foreach (Point pt in pts)
        //                if (maxx < pt.X)
        //                    maxx = pt.X;

        //            maxy = pts[0].Y;
        //            foreach (Point pt in pts)
        //                if (maxy < pt.Y)
        //                    maxy = pt.Y;

        //            rect.X = minx;
        //            rect.Y = miny;
        //            rect.Width = maxx-minx;
        //            rect.Height = maxy - miny;
        //            return rect;
        //        }
        //        public static Rectangle[] RotateRects(int W, int H, Rectangle[] rects, double degrees)
        //        { 
        //            Rectangle[] newRects = new Rectangle[rects.Length];
        //            for (int i = 0; i < rects.Length; i++)
        //            { 
        //                Point []pts = RectToPoints(rects[i]);
        //                double angle    = Math.PI * degrees / 180.0;
        //                pts  = RotateRect(W,H,pts,angle);
        //                newRects[i] = PointsToRects(pts);
        //            }
        //            return newRects;
        //        }

        //        public static byte[] RotateImageBuffer(byte[] image, ref int W, ref int H, int Ang)
        //        {
        //            byte[] dst = new byte[image.Length];
        //            if(image.Length != W*H)
        //                throw new Exception("Stride FOUND");

        //            switch(Ang)
        //            {
        //                case 90:
        //                    {
        //                        int pos = 0;
        //                        for (int x = 0; x < W; x++)
        //                            for (int y = H - 1; y >= 0; y--)
        //                                dst[pos++] = image[x + y * W];
        //                        pos = W;
        //                        W = H;
        //                        H = pos;
        //                    }
        //                    break;
        //                case 180:
        //                    {
        //                        int pos = 0;
        //                        for (int y = H - 1; y >= 0; y--)
        //                            for (int x = W-1; x >= 0; x--)
        //                                dst[pos++] = image[x + y * W];
        //                    }
        //                    break;
        //                case 270:
        //                    {
        //                        int pos = 0;
        //                        for (int x = W - 1; x >= 0; x--)
        //                            for (int y = 0; y < H; y++)
        //                                dst[pos++] = image[x + y * W];
        //                        pos = W;
        //                        W = H;
        //                        H = pos;
        //                    }
        //                    break;
        //                case 0:
        //                case 360:
        //                    {
        //                        for (int x = 0; x < image.Length; x++)
        //                                dst[x] = image[x];
        //                    }
        //                    break;
        //                default:
        //                    throw new Exception("Wrong Angle for roatation");
        //            }
        //            return dst;
        //        }
        //        public static Rectangle[] ScaleRects(Rectangle[] rects, float WidthSF, float HeightSF)
        //        {
        //            if(rects == null || rects.Length == 0)
        //                return null;
        //            List<Rectangle> lst = new List<Rectangle>();
        //            foreach (Rectangle r in rects)
        //            {
        //                Rectangle rNew = new Rectangle((int)(r.X / WidthSF), (int)(r.Y / HeightSF), (int)(r.Width / WidthSF), (int)(r.Height / HeightSF));
        //                lst.Add(rNew);
        //            }
        //            return lst.ToArray();
        //        }
        //        public static Rectangle ScaleRect(Rectangle rect, float WidthSF, float HeightSF)
        //        {
        //                return new Rectangle((int)(rect.X / WidthSF), (int)(rect.Y / HeightSF), (int)(rect.Width / WidthSF), (int)(rect.Height / HeightSF));
        //        }
    }
    
    public class FooUtils
    {
        static bool logEnabled = false;
        long funcStartTime = 0;
        Stopwatch swFoo;
        public void FooStart(string fooName)
        {
            if (logEnabled == false)
                return;
            swFoo = new Stopwatch(); swFoo.Start();
            funcStartTime = DateTime.Now.Ticks;
            Trace.TraceInformation("{0} {1} Start @ {2} ", DateTime.Now.ToString(), fooName, funcStartTime);
        }
        public void FooEnd(string fooName)
        {
            if (logEnabled == false)
                return;
            swFoo.Stop();
            long funcEndTime = DateTime.Now.Ticks;
            long timeDiff = REDTR.UTILS.Utils.GetTimeDiff_MS(swFoo);
            Trace.TraceInformation("{0} {1} End   @ {2} Diff {3}", DateTime.Now.ToString(), fooName, funcEndTime, timeDiff);
        }
    }
    
    public class DirectoryEx
    {
        public static string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }
        public static string[] GetFiles(string pathName, string validExtensions)
        {
            string[] patterns = validExtensions.Split(new char[] { ',' });

            StringCollection coll = new StringCollection();

            foreach (string pattern in patterns)
            {
                string[] files = Directory.GetFiles(pathName, pattern);
                coll.AddRange(files);
            };

            string[] arr = new string[coll.Count];
            coll.CopyTo(arr, 0);
            return arr;
        }
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }

    public static class RedMemberInfo
    {
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }
        public static Type GetMemberType<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.DeclaringType;
        }
    }
    
    public class ItemsOrganisation : IComparable
    {
        public Object Item;
        public int Pos;

        public ItemsOrganisation(Object argx, int argy)
        {
            Item = argx;
            Pos = argy;
        }

        public int CompareTo(object obj)
        {
            int result = 1;
            if (obj != null && obj is ItemsOrganisation)
            {
                ItemsOrganisation w = obj as ItemsOrganisation;
                result = this.Pos.CompareTo(w.Pos);
            }
            return result;
        }

        public static int Compare(ItemsOrganisation x, ItemsOrganisation y)
        {
            int result = 1;
            if (x != null && y != null)
            {
                result = x.CompareTo(y);
            }
            return result;
        }

        public static void InsertionSort<T>(IList<T> list, Comparison<T> comparison)
        {
            int count = list.Count;
            for (int j = 1; j < count; j++)
            {
                T key = list[j];

                int i = j - 1;
                for (; i >= 0 && comparison(list[i], key) > 0; i--)
                {
                    list[i + 1] = list[i];
                }
                list[i + 1] = key;
            }
        }
    }
    
    public class SerializeStatic
    {
        public static bool Save(Type static_class, string filename, bool hasBinary)
        {
            try
            {
                FieldInfo[] fields = static_class.GetFields(BindingFlags.Static | BindingFlags.Public);
                object[,] a = new object[fields.Length, 2];
                int i = 0;
                foreach (FieldInfo field in fields)
                {
                    a[i, 0] = field.Name;
                    a[i, 1] = field.GetValue(null);
                    i++;
                };
                Stream f = File.Open(filename, FileMode.Create);

                if (hasBinary == false)
                {
                    SoapFormatter formatter = new SoapFormatter();
                    formatter.Serialize(f, a);
                }
                else
                {
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(f, a);
                }
                f.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load(Type static_class, string filename, bool hasBinary)
        {
            try
            {
                FieldInfo[] fields = static_class.GetFields(BindingFlags.Static | BindingFlags.Public);
                object[,] a;
                Stream f = File.Open(filename, FileMode.Open);
                if (hasBinary == false)
                {
                    SoapFormatter formatter = new SoapFormatter();
                    a = formatter.Deserialize(f) as object[,];
                }
                else
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    a = formatter.Deserialize(f) as object[,];
                }
                f.Close();
                if (a.GetLength(0) != fields.Length) return false;
                int i = 0;
                foreach (FieldInfo field in fields)
                {
                    if (field.Name == (a[i, 0] as string))
                    {
                        field.SetValue(null, a[i, 1]);
                    }
                    i++;
                };
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}


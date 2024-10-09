using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    [Serializable]
    public class ConfigPara
    {

        public string PLCIPAdder { set; get; }
        public string DbIPAdder { set; get; }
       // public string RobotGipper { set; get; }
        public string PLCStruct { set; get; }
        public string DataIP { set; get; }
        public int DataPort { set; get; }

       
        public string AdminPwd { set; get; }

        public string ExeType { set; get; }

        public Robots Robots { set; get; }

       


        public static T DeepClone<T>(T config)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, config);
                ms.Seek(0, SeekOrigin.Begin);
                retval = bf.Deserialize(ms);
                ms.Close();
            }

            return (T)retval;


        }

    }
    public class KukaPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    [Serializable]
    public class RobotConfigPara
    {


        public string Point1 { set; get; }
        public string Point2 { set; get; }
        public string Point3 { set; get; }
        public string PointHeight { set; get; }

        public string AbcPre_Y { set; get; }
        public string AbcPre_Z { set; get; }

        public string XyzPre_X { set; get; }
        public string XyzPre_Y { set; get; }
        public string TimeBase { set; get; }

       

        public string StartY { set; get; }
        public string StartZ { set; get; }

        public string StartXX { set; get; }
        public string StartYY { set; get; }
    }
    [Serializable]
    public class Robots
    {
        public string BU { get; set; }
        public string LineName { get; set; }
        public string WorkName { get; set; }
        public string RobotType { get; set; }
        public string RobotIP { get; set; }
        public int RobotBas { get; set; }
        public string RobotExcelAdress { get; set; }
        //机器人的初始化文件地址
        public string RobotSeriorNo { get; set; }
        public int RobotGipperNum { get; set; }
        //机器人的初始化文件地址
        public string RobotStatNo { get; set; }
        public string RobotName { get; set; }
        public double CircDiameter { set; get; }

        public string AdjDownHeight { set; get; }
        public int AdjustmentZ { set; get; }

        public int CaTool { set; get; }

        public int ProSpeed { set; get; }
        public string CheckToolX { set; get; }
        public string CheckToolY { set; get; }
        public string CheckToolZ { set; get; }
        public string CheckToolA { set; get; }
        public string CheckToolB { set; get; }
        public string CheckToolC { set; get; }
        

        //机器人的excel的文件地址
       
        public  List<Tool> Tools { set; get; }
       

    }
    [Serializable]
    public class aGipper
    {
        public int OpenInput { get; set; }
        public int CloseInput { get; set; }
        public int OpenOut { get; set; }
        public int CloseOut { get; set; }
    }

    public class Circle
    {
        public KukaPoint Center { get; set; }
        public double Radius { get; set; }
    }
    [Serializable]
    public class Tool
    {
        public int CaToolNum { set; get; }
        public string toolA { set; get; }
        public string toolB { set; get; }
        public string toolC { set; get; }
        public string toolX { set; get; }
        public string toolY { set; get; }
        public string toolZ { set; get; }
        public string UpdateTime { get; set; }
        public string BeforUpdateTime { get; set; }
        public aGipper aGipper { set; get; }
        public RobotConfigPara RobotConfigPara { set; get; }
    }

    #region 算法 求圆心以及半径
    ////将四个三维点传递给CircleFitter对象，
    ////并使用最小二乘法拟合平面圆。FitCircle()方法返回Circle3D对象，
    ////其中包含了计算出的圆心坐标和半径。
    ////请注意，此处没有显式地投影三维点到二维投影平面上，
    ////而是直接针对三维点进行迭代计算。这是因为最小二乘法本身就能够处理带有误差的三维坐标数据，
    ////而映射到二维平面上只是为了简化计算，无法消除由于映射引入的额外误差。

    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }



    ///// <summary>
    ///// 定义输入输出的结果集
    ///// </summary>
    //public class Circle3D
    //{
    //    public Point3D CenterPoint { get; set; }
    //    public double Radius { get; set; }

    //    public Circle3D(Point3D centerPoint, double radius)
    //    {
    //        CenterPoint = centerPoint;
    //        Radius = radius;
    //    }
    //}

    //public class CircleFitter
    //{
    //    private readonly List<Point3D> _points = new List<Point3D>();
    //    private double _radius;
    //    private Point3D _centerPoint;

    //    public void AddPoint(Point3D point)
    //    {
    //        _points.Add(point);
    //    }


    //    //这里采用的具体方法是使用坐标轴上的点坐标平均值作为圆心位置进行迭代拟合
    //    //实际应用中可能需要考虑拟合精度、误差和效率等问题。
    //    public Circle3D FitCircle()
    //    {

    //        //获取数量 
    //        var n = _points._AutoStep;

    //        //计算出所有点在三个坐标轴上的平均值，作为初步拟合后的圆心位置。
    //        // 迭代计算平面圆，并依次更新圆心和半径
    //        var x = _points.Select(p => p.X).Average();
    //        var y = _points.Select(p => p.Y).Average();
    //        var z = _points.Select(p => p.Z).Average();
    //        _centerPoint = new Point3D(x, y, z);
    //        //表示该平面距离其圆心所有点到圆心的距离平方的和。
    //        double dsqSum = 0;
    //        //处理点集分别计算该点到已拟合的圆心位置的偏移量并更新 dsqSum 的值。
    //        for (var i = 0; i < n; i++)
    //        {
    //            var dx = _points[i].X - _centerPoint.X;
    //            var dy = _points[i].Y - _centerPoint.Y;
    //            var dz = _points[i].Z - _centerPoint.Z;
    //            dsqSum += dx * dx + dy * dy + dz * dz;
    //        }
    //        //利用所有计算出的点到圆心距离平方的和除以点数
    //        //n得到平均距离然后开放平方根即可获得该平面的半径大小。
    //        _radius = Math.Sqrt(dsqSum / n);

    //        return new Circle3D(_centerPoint, _radius);
    //    }
    //}
    #endregion

    public class PointAndTime
    {
        public DateTime TimeMillos { get; set; }
        public string IsWhat { get; set; }
    }


    public class PointAndTimeType
    {
        public DateTime TimeMillos { get; set; }
        public string IsWhat { get; set; }
    }


    public class ToolAbc
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
    }

    public class ToolXYZ
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class KukaVar
    {
        public string KukaMode { get; set; }
        public string KukaProState { get; set; }
        public string KukaAct { get; set; }
        public string[] Kukatool { get; set; }
    }
}

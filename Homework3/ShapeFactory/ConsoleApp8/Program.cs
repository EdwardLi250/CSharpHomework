using System;

namespace ConsoleApp7
{

    abstract class Shape
    {
        public abstract double getArea();
        public abstract bool isLegal();
    }
    class Rectangle : Shape
    {
        protected double length, width;
        public double Length
        {
            get => length;
            set => length = value;
        }
        public double Width
        {
            get => width;
            set => width = value;
        }

        public override double getArea()
        {
            if (isLegal())
            {
                return length * width;
            }
            else return -1;
        }
        public override bool isLegal()
        {
            if (length > 0 && width > 0) return true;
            else return false;
        }
    }
    class Square : Shape
    {
        protected double length;
        public double Length
        {
            get => length;
            set => length = value;
        }
        public override double getArea()
        {
            if (isLegal())
            {
                return length * length;
            }
            else return -1;
        }
        public override bool isLegal()
        {
            if (length > 0) return true;
            else return false;
        }
    }
    class Triangle : Shape
    {
        protected double edge1, edge2, edge3;
        public double Edge1
        {
            get => edge1;
            set => edge1 = value;
        }
        public double Edge2
        {
            get => edge2;
            set => edge2 = value;
        }
        public double Edge3
        {
            get => edge3;
            set => edge3 = value;
        }
        public override double getArea()
        {
            if (isLegal())
                return 1 / 4 * Math.Sqrt((edge1 + edge2 + edge3) * (edge1 + edge2 - edge3) * (edge1 + edge3 - edge2) * (edge2 + edge3 - edge1));
            else return -1;
        }
        public override bool isLegal()
        {
            if (edge1 > 0 && edge2 > 0 && edge3 > 0
                && edge1 + edge2 > edge3 && edge1 + edge3 > edge2 && edge2 + edge3 > edge1)
                return true;
            else return false;
        }
    }
    class Circle : Shape
    {
        protected double radius;
        public double Radius
        {
            get => radius;
            set => radius = value;
        }
        public override double getArea()
        {
            if (isLegal())
            {
                return radius * radius * 3.14;
            }
            else return -1;
        }
        public override bool isLegal()
        {
            if (radius > 0) return true;
            else return false;
        }
    }

    class ShapeFactory
    {
        public static Shape CreateShape(string shapeop)
        {
            if (shapeop == "rectangle")
            {
                Rectangle shape_produced1 = new Rectangle();
                Console.WriteLine("生成了一个长方形，请输入其长：");
                shape_produced1.Length = Double.Parse(Console.ReadLine());
                Console.WriteLine("请输入其宽：");
                shape_produced1.Width = Double.Parse(Console.ReadLine());
                return shape_produced1;
            }
            if (shapeop == "square")
            {
                Square shape_produced2 = new Square();
                Console.WriteLine("生成了一个正方形，请输入其边长：");
                shape_produced2.Length = Double.Parse(Console.ReadLine());
                return shape_produced2;
            }
            if (shapeop == "triangle")
            {
                Triangle shape_produced3 = new Triangle();
                Console.WriteLine("生成了一个三角形，请输入其一条边长：");
                shape_produced3.Edge1 = Double.Parse(Console.ReadLine());
                Console.WriteLine("请输入其另一条边长：");
                shape_produced3.Edge2 = Double.Parse(Console.ReadLine());
                Console.WriteLine("请输入其另一条边长：");
                shape_produced3.Edge3 = Double.Parse(Console.ReadLine());
                return shape_produced3;
            }
            if (shapeop == "circle")
            {
                Circle shape_produced4 = new Circle();
                Console.WriteLine("生成了一个圆形，请输入其半径：");
                shape_produced4.Radius = Double.Parse(Console.ReadLine());
                return shape_produced4;
            }
            else
            {
                Console.WriteLine("非法输入！");
                return null;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string[] shapeops = new string[] { "rectangle", "square", "triangle", "circle" };
            Shape[] shapes = new Shape[10];
            Random r = new Random();
            double totalArea = 0;
            for (int i = 0; i < 10; i++)
            {
                int randomNum = r.Next(0, 4);
                shapes[i] = ShapeFactory.CreateShape(shapeops[randomNum]);
                totalArea += shapes[i].getArea();
            }
            Console.WriteLine("/n总面积为：" + totalArea);
            Console.ReadKey();
            return;
        }
    }
}


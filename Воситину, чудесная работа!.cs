using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CSharp_Shell
{

    public static class Program 
    {
    	
    	static int[] Array_Del(int[] a,int k){
    		for (int i = k-1; i< a.Length; i++){
    			if (i == a.Length-1)
    			a[i] = 0;
    			else
    			a[i] = a[i+1];
    		}
    		return a;
    	}
    	static int N_eq(int[] a){
    		int N_e = 0;
    		for (int i=0; i<a.Length-1; i++){
    			if(a[i]==(-1)*a[i+1])
    			N_e++;
    		}
    		return N_e;
    	}

    	
        public static void Main() 
        {
        	do{
           try{
           	int[] a;
           Console.Write("Как хотите задать масив? (1 - вручную, другое число - рандомно): ");
           if (int.Parse(Console.ReadLine()) == 1){
            List <int> h = new List<int>();
            Console.WriteLine("Вводите числа по одному (для окончания ввода введите не число): ");
            try{
            while(true){
            	h.Add(int.Parse(Console.ReadLine()));}
            }
            catch{
            	a = new int[h.Count];
            	Console.WriteLine("Ок, ввод окончен. Ваш масив: ");
            	for(int i = 0; i<h.Count; i++){
            		a[i]=h[i];
            		Console.Write(a[i]+ "  ");
            	}
            }
            Console.WriteLine();
           }
           else{
           Console.Write("Введите кол-во элементов масива (не меньше 3): ");
           int N = int.Parse(Console.ReadLine());
           a = new int[N];
           Random rand = new Random();
           Console.Write("Нижняя грань разброса рандома: ");
           int min = int.Parse(Console.ReadLine());
           Console.Write("Верхняя грань разброса рандома (должна быть больше нижней): ");
           int max = int.Parse(Console.ReadLine())+1;
           Console.WriteLine("\nВаш масив: ");
           for(int i = 0; i<N; i++)
           {
           a[i] = rand.Next(min, max);
           Console.Write(a[i]+"  ");
           }
           Console.WriteLine();}
           Console.Write("Масив с удаленным третим элементом: ");
           Array_Del(a, 3);
           Array.Resize(ref a, a.Length-1);
           for (int i = 0; i< a.Length; i++){
           Console.Write(a[i] + "  ");}
           Console.WriteLine();
           Console.Write("Какой элемент хотите удалить? (удалит максимальный, если номер больше номера последнего элемента): ");
           int k = int.Parse(Console.ReadLine());
           Array_Del(a, k);
           Array.Resize(ref a, a.Length-1);
           Console.WriteLine("Новий масив: ");
           for (int i = 0; i< a.Length; i++){
           Console.Write(a[i] + "  ");
           }
           Console.WriteLine("Количество одинаковых за абсолютным значением, но разных по знаку пар: " + N_eq(a));
           }
           catch{
           	Console.WriteLine("Что-то пошло не так");
           }
           Console.Write("Хотите продолжить?('+' - да, [что-то другое] - нет):");
           if (Console.ReadLine()!="+")
           break;
           Console.WriteLine("\n\n");
        	}while(true);
        	
        }
    }
}
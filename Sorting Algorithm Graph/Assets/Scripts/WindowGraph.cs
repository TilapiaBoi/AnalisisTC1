﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform indexContainer;

    void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        indexContainer = transform.Find("IndexContainer").GetComponent<RectTransform>();
        int[] arraySizes = { 500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000 };
        Dictionary<int, List<double>> valueListBubbleSort = SortTester.SortTest(arraySizes, true);
        Dictionary<int, List<double>> valueListQuickSort = SortTester.SortTest(arraySizes, false);
        ShowIndex(arraySizes);
        ShowGraph(valueListBubbleSort, Color.green);
        ShowGraph(valueListQuickSort, Color.cyan);
        CreateCircle(new Vector2(450, 615), Color.cyan);
        CreateCircle(new Vector2(580, 615), Color.green);
    }

    void CreateCircle(Vector2 anchoredPosition, Color color)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(20, 20);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    void ShowIndex(int[] arraySizes)
    {
        float xSize = ((graphContainer.sizeDelta.x - 20) / arraySizes.Length);
        float yMax = 600000;
        float timeValues = 15;
        float ySize = ((graphContainer.sizeDelta.y - 5) / timeValues);
        int counter = 0;
        float xPos;
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        foreach (int arraySize in arraySizes)
        {
            xPos = 80 + 20 + counter * xSize;
            GameObject gameObject = new GameObject("xIndex", typeof(RectTransform));
            gameObject.transform.SetParent(indexContainer, false);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(xPos, 30);
            rectTransform.sizeDelta = new Vector2(50, 30);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            Text xIndex = gameObject.AddComponent<Text>();
            xIndex.text = arraySize.ToString();
            xIndex.font = ArialFont;
            xIndex.fontSize = 18;
            xIndex.material = ArialFont.material;
            counter++;
        }
        counter = 0;
        while (counter <= timeValues)
        {
            float yPos = 60 + 5 + counter * ySize;
            GameObject gameObject = new GameObject("yIndex", typeof(RectTransform));
            gameObject.transform.SetParent(indexContainer, false);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(40, yPos);
            rectTransform.sizeDelta = new Vector2(60, 30);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            Text yIndex = gameObject.AddComponent<Text>();
            yIndex.text = ((yMax / timeValues) * counter).ToString();
            yIndex.font = ArialFont;
            yIndex.fontSize = 16;
            yIndex.material = ArialFont.material;
            counter++;
        }
    }

    void ShowGraph(Dictionary<int, List<double>> valueList, Color color)
    {
        UnityEngine.Debug.Log($"Inicio de Grafico");
        float xSize = ((graphContainer.sizeDelta.x - 20) / valueList.Count);
        float yMax = 600000;
        float graphHeight = graphContainer.sizeDelta.y;
        int counter = 0;
        float xPos;
        String dataStringTot = "";
        foreach (KeyValuePair<int, List<double>> entry in valueList)
        {
            xPos = 20 + counter * xSize;
            String dataString = "";
            foreach (double data in entry.Value)
            {
                dataString = data.ToString() + " | ";
                float dataFloat = (float) data;
                float yPos = 5 + (dataFloat / yMax) * graphHeight;
                CreateCircle(new Vector2(xPos, yPos), color);
            }
            dataStringTot += dataString;
            counter++;
        }
        UnityEngine.Debug.Log(dataStringTot);
    }
}

public class SortTester
{
    private static void BubbleSort(int[] arr)
    {
        for (int i = 1; i <= arr.Length - 1; ++i)
            for (int j = 0; j < arr.Length - i; ++j)
                if (arr[j] > arr[j + 1])
                    Swap(ref arr[j], ref arr[j + 1]);
    }

    private static void Swap(ref int x, ref int y)
    {
        int temp = x;
        x = y;
        y = temp;
    }

    private static void Quick_Sort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right);
            if (pivot > 1)
            {
                Quick_Sort(arr, left, pivot - 1);
            }
            if (pivot + 1 < right)
            {
                Quick_Sort(arr, pivot + 1, right);
            }
        }

    }

    private static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[left];
        while (true)
        {
            while (arr[left] < pivot)
            {
                left++;
            }
            while (arr[right] > pivot)
            {
                right--;
            }
            if (left < right)
            {
                if (arr[left] == arr[right]) return right;
                int temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
            }
            else
            {
                return right;
            }
        }
    }

    static private double RandomSort(int arrayLenght, Boolean sortingMethod)
    {
        Stopwatch stopwatch;
        int i;
        System.Random random = new System.Random();
        int[] arr = new int[arrayLenght];
        long ticks;
        for (i = 0; i < arrayLenght; i++)
        {
            arr[i] = random.Next(0, 10000);
        }
        if (sortingMethod)
        {
            stopwatch = Stopwatch.StartNew();
            BubbleSort(arr);
            stopwatch.Stop();
            ticks = stopwatch.ElapsedTicks;
        }
        else
        {
            stopwatch = Stopwatch.StartNew();
            Quick_Sort(arr, 0, arrayLenght - 1);
            stopwatch.Stop();
            ticks = stopwatch.ElapsedTicks;
        }
        double microseconds = ((double)ticks / Stopwatch.Frequency) * 1000000.0;
        microseconds = Math.Round(microseconds, 2);
        return microseconds;
    }

    static public Dictionary<int, List<double>> SortTest(int[] arr, Boolean sortingMethod)
    {
        Dictionary<int, List<double>> dict = new Dictionary<int, List<double>>();
        Boolean first_cycle = true;
        int i = 0;
        while (i < arr.Length)
        {
            List<double> timeValues = new List<double>();
            if (first_cycle)
            {
                first_cycle = false;
                double time = RandomSort(arr[i], sortingMethod);
            }
            else
            {
                for (int cont = 0; cont < 5; cont++)
                {
                    double time = RandomSort(arr[i], sortingMethod);
                    timeValues.Add(time);
                }
                dict.Add(arr[i], timeValues);
                i++;
            }
        }
        return dict;
    }
}

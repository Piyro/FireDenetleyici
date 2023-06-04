using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductManager : MonoBehaviour
{
    [Serializable]
    public class Product
    {
        public string name;
        public string SKT;
        public string barcode;
        public bool isRed;
    }

    public List<Product> products;
    public InputField nameInput;
    public InputField SKTInput;
    public InputField barcodeInput;
    public InputField indexInput;
    public Text productListText;

    private void Start()
    {
        products = new List<Product>();

        DisplayProducts();
    }

    public void AddProduct()
    {
        string name = nameInput.text;
        string SKT = SKTInput.text;
        string barcode = barcodeInput.text;

        Product newProduct = new Product { name = name, SKT = SKT, barcode = barcode, isRed = false };

        TimeSpan remainingTime = DateTime.Parse(newProduct.SKT) - DateTime.Now;
        if (remainingTime.Days <= 5)
        {
            newProduct.isRed = true;
        }

        products.Add(newProduct);
        DisplayProducts();

        Debug.Log("Product added. Index: " + (products.Count - 1));
    }

    public void RemoveProduct()
    {
        int index = int.Parse(indexInput.text) - 1;

        if (index >= 0 && index < products.Count)
        {
            products.RemoveAt(index);
            DisplayProducts();
        }
        else
        {
            Debug.Log("Invalid index!");
        }
    }

    public void SaveProducts()
    {
        ProductData data = new ProductData { productList = products };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("ProductData", json);
        Debug.Log("Products saved. Path: " + Application.persistentDataPath);
    }

    public void LoadProducts()
    {
        if (PlayerPrefs.HasKey("ProductData"))
        {
            string json = PlayerPrefs.GetString("ProductData");
            ProductData data = JsonUtility.FromJson<ProductData>(json);
            products = data.productList;
            DisplayProducts();
            Debug.Log("Products loaded. Path: " + Application.persistentDataPath);
        }
        else
        {
            Debug.Log("No saved products found.");
        }
    }

    private void DisplayProducts()
    {
        productListText.text = "";

        for (int i = 0; i < products.Count; i++)
        {
            Product product = products[i];

            string colorTag = product.isRed ? "<color=red>" : "";
            string colorTagClose = product.isRed ? "</color>" : "";

            productListText.text += $"{i + 1}. {colorTag}{product.name}{colorTagClose}: ";
            productListText.text += $"SKT: {product.SKT} ";
            productListText.text += $"Barkod: {product.barcode}\n";
        }
    }

    [Serializable]
    public class ProductData
    {
        public List<Product> productList;
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

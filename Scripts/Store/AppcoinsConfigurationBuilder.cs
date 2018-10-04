using System.Collections;
using System.Collections.Generic;
using Appcoins.Purchasing;
using UnityEngine;

public class AppcoinsConfigurationBuilder : MonoBehaviour {

    public List<AppcoinsProduct> products;

    private void Awake()
    {
        if (products == null)
            products = new List<AppcoinsProduct>();
    }

    public void AddProduct(string skuID, AppcoinsProductType type)
    {
        AppcoinsProduct product = new AppcoinsProduct();
        product.skuID = skuID;
        product.productType = type;

        if (products == null)
            products = new List<AppcoinsProduct>();
        products.Add(product);
    }
}

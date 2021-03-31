﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
	public static IAPManager instance;

	private static IStoreController m_StoreController;
	private static IExtensionProvider m_StoreExtensionProvider;
	
	public static string PRODUCT_2000 = "gems_2000";  
	public static string PRODUCT_5000 = "gems_5000";  
	public static string PRODUCT_20000 = "gems_20000";  
	public static string PRODUCT_50000 = "gems_50000";  
	public static string PRODUCT_100000 = "gems_100000"; 
	public static string PRODUCT_500000 = "gems_500000"; 
	public static string PRODUCT_NOADS = "no_ads";    
					
	private void Awake()
	{		
		if (instance != null) Destroy(gameObject);
		else { instance = this; DontDestroyOnLoad(gameObject); }

		if (m_StoreController == null)
			InitializePurchasing();
	}
	
	public void InitializePurchasing() 
	{
		if (IsInitialized())
			return;
		
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		
		builder.AddProduct(PRODUCT_2000, ProductType.Consumable);
		builder.AddProduct(PRODUCT_5000, ProductType.Consumable);
		builder.AddProduct(PRODUCT_20000, ProductType.Consumable);
		builder.AddProduct(PRODUCT_50000, ProductType.Consumable);
		builder.AddProduct(PRODUCT_100000, ProductType.Consumable);
		builder.AddProduct(PRODUCT_500000, ProductType.Consumable);
		builder.AddProduct(PRODUCT_NOADS, ProductType.Consumable);
		
		UnityPurchasing.Initialize(this, builder);
	}
	
	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}
		
	public void BuyGems(int gems)
	{
		switch(gems)
		{
			case 2000: BuyProductID(PRODUCT_2000); break;
			case 5000: BuyProductID(PRODUCT_5000); break;
			case 20000: BuyProductID(PRODUCT_20000); break;
			case 50000: BuyProductID(PRODUCT_50000); break;
			case 100000: BuyProductID(PRODUCT_100000); break;
			case 500000: BuyProductID(PRODUCT_500000); break;

			case 0: BuyProductID(PRODUCT_NOADS); break;
		}
	}

		
	private void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Debug.Log("BuyProductID: " + "IsInitialized: true");
			Product product = m_StoreController.products.WithID(productId);
			
			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else
			{
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");
		
		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}
		
	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}
		
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_2000, StringComparison.Ordinal))
		{
			PurchaseCompleted(2000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_5000, StringComparison.Ordinal))
		{
			PurchaseCompleted(5000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_20000, StringComparison.Ordinal))
		{
			PurchaseCompleted(20000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_50000, StringComparison.Ordinal))
		{
			PurchaseCompleted(50000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_100000, StringComparison.Ordinal))
		{
			PurchaseCompleted(100000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_500000, StringComparison.Ordinal))
		{
			PurchaseCompleted(500000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_NOADS, StringComparison.Ordinal))
		{
			PurchaseCompleted(0);
		}
		else 
		{
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}

		return PurchaseProcessingResult.Complete;
	}
		
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}

	private void PurchaseCompleted(int gems)
	{
		if(gems == 0)
			FindObjectOfType<ShopManager>().BuyNoAdsSuccess();
		else
			FindObjectOfType<ShopManager>().BuyGemSuccess(gems);
	}

}


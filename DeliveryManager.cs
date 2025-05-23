using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set;}
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;
    private void Awake(){
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update(){
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;
            
            if (waitingRecipeSOList.Count < waitingRecipesMax) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++){
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count){
                //Has same number of ingridients
                bool plateContentsMatchesRecipe = true;
                
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    //cycle through all ingredients in the Recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        //cycle through all ingredients on the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            //ingredients match!
                            ingredientFound = true;
                            break;
                        }
                    }
                    
                    if (!ingredientFound) {
                        //this Recipe ingredient was not found on the Plate;
                        plateContentsMatchesRecipe = false;
                    }
                }
                
                if (plateContentsMatchesRecipe) {
                    //Player delivered the correct Recipe!
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    successfulRecipesAmount++;
                    return;
                }
            }
        }
        //No matches found! Incorrect Recipe!
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }
    //AddRecipeSO был заменен на более гибкий подход. Не знаю че я думал когда написал изначально ремув и адд вместо простого геттера
    public int GetSuccessfulRecipesAmount() {
        return successfulRecipesAmount;
    }
}

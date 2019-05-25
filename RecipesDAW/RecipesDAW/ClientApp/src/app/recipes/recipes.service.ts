import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Recipe } from './recipes.models';

@Injectable()
export class RecipesService {

  constructor(
    private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  listRecipes() {
    return this.http.get<Recipe[]>(this.baseUrl + 'api/Recipes/ListRecipes');
  }

  loadRecipe(recipeID: string) {
    return this.http.get<Recipe>(this.baseUrl + `api/Recipes/DetailRecipe?recipeID=${recipeID}`);
  }

  saveRecipe(recipe: Recipe) {
    return this.http.post<any>(this.baseUrl + `api/Recipes/SaveRecipe`, recipe);
  }

  deleteRecipe(recipeID: string) {
    return this.http.delete<any>(this.baseUrl + `api/Recipes/DeleteRecipe?recipeID=${recipeID}`);
  }
}

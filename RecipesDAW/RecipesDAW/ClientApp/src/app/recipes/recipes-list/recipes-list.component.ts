import { Component, OnInit } from '@angular/core';

import { Recipe } from './../recipes.models';

import { RecipesService } from './../recipes.service';


@Component({
  selector: 'app-recipes-list',
  templateUrl: './recipes-list.component.html',
  styleUrls: ['./recipes-list.component.css']
})
export class RecipesListComponent implements OnInit {

  public recipes: Recipe[];

  constructor(private recipesService: RecipesService) {
}

  ngOnInit() {
    this.loadRecipes();
  }

  deleteRecipe(recipe: Recipe) {
    this.recipesService.deleteRecipe(recipe.id).subscribe(x => {
      this.loadRecipes();
    });
  }

  loadRecipes() {
    this.recipesService.listRecipes().subscribe(res => {
      this.recipes = res;
    })
  }
}

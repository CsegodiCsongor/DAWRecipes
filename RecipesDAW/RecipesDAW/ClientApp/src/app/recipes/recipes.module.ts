import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CoreModule } from '../core/core.module';

import { AngularMaterialModule } from '../shared/angular-material.module';

import { RecipesService } from './recipes.service';

import { RecipesRoutingModule } from './recipes-routing.module';

@NgModule({
  declarations: [RecipesRoutingModule.routedComponents],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    AngularMaterialModule,
    ReactiveFormsModule,
    RecipesRoutingModule
  ],
  providers: [RecipesService]
})
export class RecipesModule { }

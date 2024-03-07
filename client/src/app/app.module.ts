//The imports section is used to import angular modules into the project.
//Needing to have import modules is needed to get certain functionalities from the Angular framework. 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import{HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

@NgModule({
  //Any new components are added into the declarations section
  declarations: [
    AppComponent
  ],
  //The imports section is to add custom modules and angular modules into the project
  imports: [
    BrowserModule,
    AppRoutingModule,
    //The following module allows angular to make HTTP requests to the API
    HttpClientModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

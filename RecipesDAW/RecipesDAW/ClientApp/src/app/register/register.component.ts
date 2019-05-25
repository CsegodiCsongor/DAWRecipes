import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { RegisterModel, Token } from '../shared/security.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  public registerModel: RegisterModel = <RegisterModel>{};

  constructor(
    private httpClient: HttpClient,
    private router: Router,
    @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
  }

  registerUser() {
    this.httpClient.post<Token>(this.baseUrl + 'api/Account/Register', this.registerModel).subscribe(token => {

      localStorage.setItem("token", JSON.stringify(token));

      this.router.navigate(['']);
    })
  }

}

import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { LoginModel, Token } from '../shared/security.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public loginModel: LoginModel = <LoginModel>{};

  constructor(
    private httpClient: HttpClient,
    private router: Router,
    @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
  }

  loginUser() {
    this.httpClient.post<Token>(this.baseUrl + 'api/Account/Login', this.loginModel).subscribe(token => {

      localStorage.setItem("token", JSON.stringify(token));

      this.router.navigate(['']);
    })
  }

}

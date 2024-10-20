import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '@shared/services/auth.service';
import { TranslationService } from '@shared/services/translation.service';
import { Router } from '@angular/router';
import { MaterialModule } from '@shared/modules/matetial/material.module';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule, 
    CommonModule, 
    MaterialModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  public authForm: FormGroup;
  isRegister = false;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, public translationService: TranslationService) {
    this.authForm = this.fb.group({
      email: ['', []],
      login: ['', []],
      username: ['', []],
      password: ['', []],
      confirmPassword: ['']
    });
  }

  ngOnInit() {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/main-page']);
    }
    this.updateValidators();
  }

  toggleMode() {
    this.isRegister = !this.isRegister;
    this.updateValidators();
  }

  updateConfirmPasswordField() {
    const confirmPasswordControl = this.authForm.get('confirmPassword');

    if (this.isRegister) {
      confirmPasswordControl?.setValidators([Validators.required, Validators.minLength(6)]);
    } 
    else {
      confirmPasswordControl?.clearValidators();
    }

    confirmPasswordControl?.updateValueAndValidity();
  }

  onSubmit() {
    if (this.authForm.invalid) {
      return;
    }

    if (this.isRegister) {
      const { email, username, password, confirmPassword } = this.authForm.value;
      if (password !== confirmPassword) {
        return;
      }
      this.authService.register(username, email, password).subscribe({
        next: () => {
          this.router.navigate(['/main-page']);
        },
        error: (error) => {
          this.errorMessage = error.status === 401
            ? this.translationService.translate('auth.WrongRegisterDataProvided')
            : this.translationService.translate('auth.UnknownAuthorizationError');
        },
      });
    } 
    else {
      const { login, password } = this.authForm.value;
      this.authService.login(login, password).subscribe({
        next: () => {
          this.router.navigate(['/main-page']);
        },
        error: (error) => {
          this.errorMessage = error.status === 401 
            ? this.translationService.translate('auth.WrongCredentialsProvided') 
            : this.translationService.translate('auth.UnknownAuthorizationError');
        },
      });
    }
  }

  updateValidators() {
    const emailControl = this.authForm.get('email');
    const loginControl = this.authForm.get('login');
    const usernameControl = this.authForm.get('username');
    const passwordControl = this.authForm.get('password');
    const confirmPasswordControl = this.authForm.get('confirmPassword');

    if (this.isRegister) {
      emailControl?.setValidators([Validators.required, Validators.email]);
      usernameControl?.setValidators([Validators.required]);
      loginControl?.clearValidators();
      passwordControl?.setValidators([Validators.required, Validators.minLength(8)]);
      confirmPasswordControl?.setValidators([Validators.required]); 
    } 
    else {
      emailControl?.clearValidators();
      usernameControl?.clearValidators();
      loginControl?.setValidators([Validators.required]);
      passwordControl?.setValidators([Validators.required]); 
      confirmPasswordControl?.clearValidators(); 
    }

    emailControl?.updateValueAndValidity();
    usernameControl?.updateValueAndValidity();
    loginControl?.updateValueAndValidity();
    passwordControl?.updateValueAndValidity();
    confirmPasswordControl?.updateValueAndValidity();
  }

  translate(key: string){
    return this.translationService.translate(key);
  }
}
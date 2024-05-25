import { Component } from '@angular/core';
import {AuthService} from "../../../services/auth.service";
import {Router} from "@angular/router";
import {error} from "@angular/compiler-cli/src/transformers/util";
import {FormBuilder, Validators} from "@angular/forms";

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.css'
})
export class RegisterUserComponent {
  username: string | null = null;
  name: string | null = null;
  password: string | null = null;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  registerForm = this.fb.group({
    username: ['', Validators.required],
    name: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  constructor (private authService: AuthService,
               private router: Router,
               private fb: FormBuilder) { }

  register() {
    if (this.registerForm.invalid) {
      this.errorMessage = 'Please fill all the fields';
      return;
    }
    this.authService.register(this.username, this.name, this.password).subscribe({
      next: () => {
        this.errorMessage = null;
        this.successMessage = 'User registered successfully. Redirecting...';
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error;
      }
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/Auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    this.authService.currenPhotoUrl.subscribe(photoUrl => { this.photoUrl = photoUrl; });
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('succesfully logged in.');
    }, error => {
      this.alertify.error('failed to login');
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }




}

import { Component, signal } from '@angular/core';
import { MainLayout } from './layout/main-layout/main-layout';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [MainLayout, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('frontend');
}

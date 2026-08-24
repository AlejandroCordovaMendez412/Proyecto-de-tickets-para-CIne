import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { LayoutComponent } from './layout/layout.component';

@NgModule({ declarations: [LayoutComponent], imports: [SharedModule, HttpClientModule], exports: [LayoutComponent] })
export class CoreModule {}

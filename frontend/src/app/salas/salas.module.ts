import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { SalasRoutingModule } from './salas-routing.module';
import { SalasComponent } from './salas.component';
@NgModule({ declarations: [SalasComponent], imports: [SharedModule, SalasRoutingModule] })
export class SalasModule {}

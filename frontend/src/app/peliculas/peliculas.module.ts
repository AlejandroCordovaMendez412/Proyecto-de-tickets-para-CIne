import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PeliculasRoutingModule } from './peliculas-routing.module';
import { PeliculasComponent } from './peliculas.component';
@NgModule({ declarations: [PeliculasComponent], imports: [SharedModule, PeliculasRoutingModule] })
export class PeliculasModule {}

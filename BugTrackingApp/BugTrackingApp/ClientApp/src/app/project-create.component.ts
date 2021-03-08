import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from './data.service';
import { Project } from './project';

@Component({
    templateUrl: './project-create.component.html'
})
export class ProjectCreateComponent {

    project: Project = new Project();    // добавляемый объект
    constructor(private dataService: DataService, private router: Router) { }
    save() {
        this.dataService.createProject(this.project).subscribe(data => this.router.navigateByUrl("/"));
    }
}
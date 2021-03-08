import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DataService } from './data.service';
import { Project } from './project';

@Component({
    templateUrl: './project-edit.component.html'
})
export class ProjectEditComponent implements OnInit {

    id: number;
    project: Project;    // изменяемый объект
    loaded: boolean = false;

    constructor(private dataService: DataService, private router: Router, activeRoute: ActivatedRoute) {
        this.id = Number.parseInt(activeRoute.snapshot.params["id"]);
    }

    ngOnInit() {
        if (this.id)
            this.dataService.getProject(this.id)
                .subscribe((data: Project) => {
                    this.project = data;
                    if (this.project != null) this.loaded = true;
                });
    }

    save() {
        this.dataService.updateProject(this.project).subscribe(data => this.router.navigateByUrl("/"));
    }
}
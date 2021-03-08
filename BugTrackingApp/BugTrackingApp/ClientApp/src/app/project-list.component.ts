import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Project } from './project';

@Component({
    templateUrl: './project-list.component.html'
})
export class ProjectListComponent implements OnInit {

    projects: Project[];
    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.load();
    }
    load() {
        this.dataService.getProjects().subscribe((data: Project[]) => this.projects = data);
    }
    delete(id: number) {
        this.dataService.deleteProject(id).subscribe(data => this.load());
    }
}


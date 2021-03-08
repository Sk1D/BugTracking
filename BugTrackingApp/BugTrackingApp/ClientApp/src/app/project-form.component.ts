import { Component, Input } from '@angular/core';
import { Project } from './project';
@Component({
    selector: "project-form",
    templateUrl: './project-form.component.html'
})
export class ProjectFormComponent {
    @Input() project: Project;
}
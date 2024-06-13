import React, { Component, Fragment } from 'react';
import { Button, Modal } from 'react-bootstrap';
import ItemForm from './CourseForm';
export default class CourseModal extends Component {
    state = {
        modal: false
    }
    toggle = () => {
        this.setState(previous => ({
            modal: !previous.modal
        }));
    }
    render() {
        const isNew = this.props.isNew;
        let title = 'Edit Course';
        let button = '';
        if (isNew) {
            title = 'Add Course';
            button = <Button
                variant="primary"
                onClick={this.toggle}
                style={{ minWidth: "200px" }}>Add</Button>;
        } else {
            button = <Button
                variant="primary"
                onClick={this.toggle}>Edit</Button>;
        }
        return <Fragment>
            {button}
            <Modal show={this.state.modal} className={this.props.className} onHide={this.toggle}>
                <Modal.Header closeButton>{title}</Modal.Header>
                <Modal.Body>
                    <ItemForm
                        addCourseToState={this.props.addCourseToState}
                        updateCourseIntoState={this.props.updateCourseIntoState}
                        toggle={this.toggle}
                        course={this.props.course} />
                </Modal.Body>
            </Modal>
        </Fragment>;
    }
}
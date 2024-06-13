import React from 'react';
import { Button, Form, Alert } from 'react-bootstrap';

export default class CourseForm extends React.Component
{
    state = {
        id: 0,
        name: '',
        lecturer: '',
        audience: '',
        start: '',
        length: '',
        alertVisible: false,
        validated: false
    }

    componentDidMount()
    {
        if (this.props.course)
        {
            const { id, name, lecturer, audience, start, length } = this.props.course
            this.setState({ id, name, lecturer, audience, start, length });
        }
    }
    onChange = e =>
    {
        this.setState({ [e.target.name]: e.target.value })
    }

    submitNew = (e) =>
    {
        e.preventDefault();

        const form = e.currentTarget;
        if (form.checkValidity() === false)
        {
            e.stopPropagation();
        }
        else
        {
            this.createCourse();
        }

        this.setState({ validated: true });
    }

    async createCourse()
    {
        fetch(`${window.CATALOG_ITEMS_API_URL}`, {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name: this.state.name,
                lecturer: this.state.lecturer,
                audience: parseInt(this.state.audience),
                start: this.state.start.toString(),
                lenght: parseInt(this.state.lenght)
            })
        })
            .then(async response =>
            {
                if (!response.ok)
                {
                    const errorData = await response.json();
                    console.error(errorData);
                    throw new Error(`Could not add the course: ${errorData.title}`);
                }

                return response.json();
            })
            .then(course =>
            {
                this.props.addCourseToState(course);
                this.props.toggle();
            })
            .catch(err => 
            {
                this.showAlert(err.message);
            });
    }

    submitEdit = e =>
    {
        e.preventDefault();

        const form = e.currentTarget;
        if (form.checkValidity() === false)
        {
            e.stopPropagation();
        }
        else
        {
            this.updateCourse();
        }

        this.setState({ validated: true });
    }

    async updateCourse()
    {
        fetch(`${window.CATALOG_ITEMS_API_URL}/${this.state.id}`, {
            method: 'put',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: this.state.id,
                name: this.state.name,
                lecturer: this.state.lecturer,
                audience: parseInt(this.state.audience),
                start: this.state.start.toString(),
                lenght: parseInt(this.state.length)
            })
        })
            .then(async response =>
            {
                if (!response.ok)
                {
                    const errorData = await response.json();
                    console.error(errorData);
                    throw new Error(`Could not update the course: ${errorData.title}`);
                }

                this.props.toggle();
                this.props.updateItemIntoState(this.state.id);
            })
            .catch(err => 
            {
                this.showAlert(err.message);
            });
    }

    showAlert = (message) =>
    {
        this.setState({
            alertMessage: message,
            alertColor: "danger",
            alertVisible: true
        });
    }

    render()
    {
        return <Form noValidate validated={this.state.validated} onSubmit={this.props.item ? this.submitEdit : this.submitNew}>
            <Form.Group>
                <Form.Label htmlFor="name">Name:</Form.Label>
                <Form.Control type="text" name="name" onChange={this.onChange} value={this.state.name} required />
                <Form.Control.Feedback type="invalid">The Name field is required</Form.Control.Feedback>
            </Form.Group>
            <Form.Group>
                <Form.Label htmlFor="lecturer">Lecturer:</Form.Label>
                <Form.Control type="text" name="lecturer" onChange={this.onChange} value={this.state.lecturer} />
            </Form.Group>
            <Form.Group>
                <Form.Label htmlFor="audience">Audience:</Form.Label>
                <Form.Control type="number" name="audience" onChange={this.onChange} value={this.state.audience} required />
                <Form.Control.Feedback type="invalid">The Audience field is required</Form.Control.Feedback>
            </Form.Group>
            <Form.Group>
                <Form.Label htmlFor="start">Start:</Form.Label>
                <Form.Control type="date" name="start" onChange={this.onChange} value={this.state.start} required />
                <Form.Control.Feedback type="invalid">The Start field is required</Form.Control.Feedback>
            </Form.Group>
            <Form.Group>
                <Form.Label htmlFor="length">Length:</Form.Label>
                <Form.Control type="number" name="length" onChange={this.onChange} value={this.state.length} required />
                <Form.Control.Feedback type="invalid">The Length field is required</Form.Control.Feedback>
            </Form.Group>
            <Button variant="primary" type="submit">Save</Button>

            <Alert style={{ marginTop: "10px" }} variant={this.state.alertColor} show={this.state.alertVisible}>
                {this.state.alertMessage}
            </Alert>
        </Form>;
    }
}
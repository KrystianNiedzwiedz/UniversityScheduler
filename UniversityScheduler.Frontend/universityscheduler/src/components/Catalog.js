import React, { Component } from 'react';
import { Col, Container, Row, Table, Button } from 'react-bootstrap';
import ItemModal from './form/CourseModal';
import GrantItemModal from './form/GrantCourseModal';

export class Catalog extends Component
{
  static displayName = Catalog.name;

  constructor(props)
  {
    super(props);
    this.state = { items: [], loading: true, loadedSuccess: false };
  }

  componentDidMount()
  {
    this.populateCourses();
  }

  async populateCourses()
  {
    fetch(`${window.CATALOG_COURSES_API_URL}`)
      .then(response => { 
        return response.json(); 
      })
      .then(returnedItems => this.setState({ courses: returnedItems, loading: false, loadedSuccess: true }))
      .catch(err =>
      {
        console.log(err);
        this.setState({ courses: [], loading: false, loadedSuccess: false })
      });
  }

  addItemToState = course =>
  {
    this.setState(previous => ({
      courses: [...previous.courses, course]
    }));
  }
  updateState = (id) =>
  {
    this.populateCourses();
  }
  deleteCourseFromState = id =>
  {
    const updated = this.state.course.filter(course => course.id !== id);
    this.setState({ courses: updated })
  }
  async deleteCourse(id)
  {
    let confirmDeletion = window.confirm('Do you really wish to delete it?');
    if (confirmDeletion)
    {
      fetch(`${window.CATALOG_COURSES_API_URL}/${id}`, {
        method: 'delete',
        headers: {
          'Content-Type': 'application/json'
        }
      })
        .then(res =>
        {
          this.deleteCourseFromState(id);
        })
        .catch(err =>
        {
          console.log(err);
          window.alert("Could not delete the course.");
        });
    }
  }

  renderCourseTable(items)
  {
    return <Container style={{ paddingTop: "10px", paddingLeft: "0px" }}>
      <Row>
        <Col>
          <Table striped bordered hover >
            <thead className="thead-dark">
              <tr>
                <th>Name</th>
                <th>Lecturer</th>
                <th>Audience</th>
                <th>Start</th>
                <th>Length</th>
                <th style={{ textAlign: "center" }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {!courses || courses.length <= 0 ?
                <tr>
                  <td colSpan="6" align="center"><b>No Courses yet</b></td>
                </tr>
                : courses.map(course => (
                  <tr key={course.id}>
                    <td>
                      {course.name}
                    </td>
                    <td>
                      {course.description}
                    </td>
                    <td>
                      {course.price}
                    </td>
                    <td align="center">
                      <div>
                        <CourseModal
                          isNew={false}
                          course={course}
                          updateCourseIntoState={this.updateState} />
                    &nbsp;&nbsp;&nbsp;
                        <GrantCourseModal
                          course={course} />
                    &nbsp;&nbsp;&nbsp;
                    <Button variant="danger" onClick={() => this.deleteCourse(course.id)}>Delete</Button>
                      </div>
                    </td>
                  </tr>
                ))}
            </tbody>
          </Table>
        </Col>
      </Row>
      <Row>
        <Col>
          <CourseModal isNew={true} addCourseToState={this.addCourseToState} />
        </Col>
      </Row>
    </Container>;
  }

  render()
  {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.state.loadedSuccess
        ? this.renderCoursesTable(this.state.courses)
        : <p>Could not load items</p>;

    return (
      <div>
        <h1 id="tabelLabel" >Catalog Courses</h1>
        {contents}
      </div>
    );
  }
}

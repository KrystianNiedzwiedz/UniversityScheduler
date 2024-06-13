import React, { Component } from 'react';
import { Col, Container, Row, Table, Button, Form } from 'react-bootstrap';

export class Inventory extends Component
{
  static displayName = Inventory.name;

  constructor(props)
  {
    super(props);
    this.state = { userId: '', courses: [], renderCourses: false, loading: true, loadedSuccess: false };
  }

  onChange = e =>
  {
    this.setState({ [e.target.name]: e.target.value })
  }

  async populateItems()
  {
    if (this.state.userId === '')
    {
      return;
    }

    this.setState({ courses: [], renderCourses: true, loading: true, loadedSuccess: false })
    fetch(`${window.INVENTORY_COURSES_API_URL}?userId=${this.state.userId}`)
      .then(response => response.json())
      .then(returnedCourses => this.setState({ items: returnedCourses, loading: false, loadedSuccess: true }))
      .catch(err =>
      {
        console.log(err);
        this.setState({ courses: [], loading: false, loadedSuccess: false })
      });
  }

  renderInputs()
  {
    return <Form inline >
      <Form.Label htmlFor="userId" srOnly>User Id:</Form.Label>
      <Form.Control
        className="mb-2 mr-sm-2"
        style={{ minWidth: "350px" }}
        type="text"
        name="userId"
        id="userId"
        placeholder="Enter a user id"
        onChange={this.onChange}
        value={this.state.userId} />
      <Button className="mb-2" variant="primary" onClick={() => this.populateCourses()}>Get Inventory</Button>
    </Form>;
  }

  renderCoursesTable()
  {
    return this.state.renderItems === false ? ''
      : this.state.loading ? <p><em>Loading...</em></p>
        : this.state.loadedSuccess === false ? <p>Could not load courses</p>
          : <Container style={{ paddingTop: "10px", paddingLeft: "0px" }}>
            <Row>
              <Col>
                <Table striped>
                  <thead className="thead-dark">
                    <tr>
                      <th>Name</th>
                      <th>Lecturer</th>
                      <th>Audience</th>
                      <th>Start</th>
                      <th>Length</th>
                    </tr>
                  </thead>
                  <tbody>
                    {!this.state.courses || this.state.courses.length <= 0 ?
                      <tr>
                        <td colSpan="6" align="center"><b>No Courses yet</b></td>
                      </tr>
                      : this.state.courses.map(item => (
                        <tr key={item.catalogItemId}>
                          <td>
                            {course.name}
                          </td>
                          <td>
                            {course.lecturer}
                          </td>
                          <td>
                            {}
                          </td>
                          <td>
                            {course.quantity}
                          </td>
                        </tr>
                      ))}
                  </tbody>
                </Table>
              </Col>
            </Row>
          </Container>;
  }

  render()
  {
    return (
      <div>
        <h1 id="tabelLabel" >Inventory</h1>
        {this.renderInputs()}
        {this.renderItemsTable()}
      </div>
    );
  }
}

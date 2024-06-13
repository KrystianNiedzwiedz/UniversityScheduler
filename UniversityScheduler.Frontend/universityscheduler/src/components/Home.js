import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { ApplicationPaths } from './Constants';

export class Home extends Component
{
  static displayName = Home.name;

  render()
  {
    return (
      <div>
        <h1>University Scheduler</h1>
      </div>
    );
  }
}

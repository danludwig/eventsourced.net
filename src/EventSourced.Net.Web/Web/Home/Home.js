import React, { Component, PropTypes } from 'react'
import {connect} from 'react-redux'
import Helmet from 'react-helmet'

export default class Home extends Component {
  render() {
    return (
      <div>
        <Helmet title="Home" />
        <div id="myCarousel" className="carousel slide" data-ride="carousel" data-interval="6000">
          <ol className="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" className="active"></li>
            <li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>
          </ol>
          <div className="carousel-inner" role="listbox">
            <div className="item active">
              <img src="/images/Banner-01-EventSourced.png" alt="EventSourced.Net" className="img-responsive" />
              <div className="carousel-caption">
                <p>
                  Get started using ASP.NET 5 with the event sourcing pattern, not Entity Framework.
                  {' '}
                  <a className="btn btn-default btn-default" href="https://github.com/danludwig/eventsourced.net">
                    Learn More
                  </a>
                </p>
              </div>
            </div>
            <div className="item">
              <img src="/images/ASP-NET-Banners-01.png" alt="ASP.NET" className="img-responsive" />
              <div className="carousel-caption">
                <p>
                  Learn how to build ASP.NET apps that can run anywhere.
                  {' '}
                  <a className="btn btn-default btn-default" href="http://go.microsoft.com/fwlink/?LinkID=525028&clcid=0x409">
                    Learn More
                  </a>
                </p>
              </div>
            </div>
            <div className="item">
              <img src="/images/ASP-NET-Banners-02.png" alt="Package Management" className="img-responsive" />
              <div className="carousel-caption">
                <p>
                  Bring in libraries from NuGet, Bower, and npm, and automate tasks using Grunt or Gulp.
                  {' '}
                  <a className="btn btn-default btn-default" href="http://go.microsoft.com/fwlink/?LinkID=525029&clcid=0x409">
                    Learn More
                  </a>
                </p>
              </div>
            </div>
          </div>
          <a className="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
            <span className="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
            <span className="sr-only">Previous</span>
          </a>
          <a className="right carousel-control" href="#myCarousel" role="button" data-slide="next">
            <span className="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
            <span className="sr-only">Next</span>
          </a>
        </div>
        <div className="row">
          <div className="col-md-3">
            <h2>Application uses</h2>
            <ul>
              <li>Event-sourced write model using <a href="http://www.geteventstore.com">EventStore</a></li>
              <li>Eventually consistent read model using <a href="https://www.arangodb.com">ArangoDB</a></li>
              <li>Web sockets to bridge the eventual consistency gap using <a href="https://github.com/sta/websocket-sharp">websocket-sharp</a></li>
              <li style={{textDecoration: 'line-through'}}>Relational impedance mismatch using <a href="http://www.asp.net/entity-framework">Entity Framework</a></li>
            </ul>
          </div>
          <div className="col-md-3">
            <h2>How to</h2>
            <ul>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=398600">Add a Controller and View</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=699314">Add an appsetting in config and access it in app.</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=699315">Manage User Secrets using Secret Manager.</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=699316">Use logging to log a message.</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=699317">Add packages using NuGet.</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=699318">Add client packages using Bower.</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=699319">Target development, staging or production environment.</a></li>
            </ul>
          </div>
          <div className="col-md-3">
            <h2>Overview</h2>
            <ul>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=518008">Conceptual overview of what is ASP.NET 5</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=699320">Fundamentals of ASP.NET 5 such as Startup and middleware.</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=398602">Working with Data</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkId=398603">Security</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=699321">Client side development</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=699322">Develop on different platforms</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=699323">Read more on the documentation site</a></li>
            </ul>
          </div>
          <div className="col-md-3">
            <h2>Run & Deploy</h2>
            <ul>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=517851">Run your app</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=517852">Run your app on .NET Core</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=517853">Run commands in your project.json</a></li>
              <li><a href="http://go.microsoft.com/fwlink/?LinkID=398609">Publish to Microsoft Azure Web Apps</a></li>
            </ul>
          </div>
        </div>
      </div>
    )
  }
}

// Home.propTypes = {
//   title: PropTypes.string.isRequired
// }

// function select(state) {
//   return {
//     title: state.getIn(['viewTitles', 'home'])
//   }
// }
//
// export default connect(select)(Home)

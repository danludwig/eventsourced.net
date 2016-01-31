import Helmet from 'react-helmet'

export default class BadRequest400 extends React.Component {
  render() {
    return(
      <div>
        <Helmet title="400 Bad Request" />
        <h1 className="text-danger">Error.</h1>
        <h2 className="text-danger">400 Bad Request</h2>
      </div>
    )
  }
}

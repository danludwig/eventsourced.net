import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

const View = ({ location: { query, }, }) => (
  <div>
    <Helmet title="Register" />
    <h2>Register.</h2>
    <h4>Create a new account.</h4>
    <hr />
    <Form onSubmit={onSubmit} initialValues={query} />
  </div>
)

View.propTypes = {
  location: React.PropTypes.shape({
    query: React.PropTypes.shape({
      returnUrl: React.PropTypes.string,
    }),
  }),
}

export default View

import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

export default ({ params, location: { query, }, }) => (
  <div>
    <Helmet title="Log in" />
    <h2>Log in.</h2>
    <h4>Use a local account to log in.</h4>
    <hr />
    <Form onSubmit={onSubmit} initialValues={{ ...params, ...query }} />
  </div>
)

import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

export default ({ params, location: { query, }, }) => (
  <div>
    <Helmet title="Verify" />
    <h2>Verify.</h2>
    <h4>Confirm your contact information.</h4>
    <hr />
    <Form onSubmit={onSubmit} initialValues={{ ...params, ...query }} />
  </div>
)

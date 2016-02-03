import { reduxForm } from 'redux-form'
import ReduxFormPropTypes from '../Shared/propTypes/reduxForm'

const form = 'logoff'
const fields = ['returnUrl']
const config = { form, fields, }

const Form = ({ handleSubmit, submitting, username, }) => (
  <form method="post" onSubmit={handleSubmit}>
    <ul className="nav navbar-nav navbar-right">
      <li>
        <a href="#/not-implemented" title="Manage">Hello {username}!</a>
      </li>
      <li>
        <button type="submit" className="btn btn-link navbar-btn navbar-link" disabled={submitting}>Log off</button>
      </li>
    </ul>
  </form>
)

Form.propTypes = {
  ...ReduxFormPropTypes.formName,
  ...ReduxFormPropTypes.handleSubmit,
  ...ReduxFormPropTypes.submitting,
  username: React.PropTypes.string.isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)

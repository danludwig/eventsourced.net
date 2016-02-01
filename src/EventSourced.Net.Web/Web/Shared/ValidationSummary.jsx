import Markdown from 'react-remarkable'

class Container extends React.Component {
  static propTypes = {
    errors: React.PropTypes.object.isRequired,
    visible: React.PropTypes.bool.isRequired,
  };

  static defaultProps = {
    visible: true,
  };

  render() {
    const messages = [], { visible, errors, } = this.props
    if (visible) {
      for (const field in errors) {
        if (!errors.hasOwnProperty(field) || !errors[field]) continue
        messages.push(errors[field])
      }
    }
    if (!messages.length) return false
    return(
      <Component messages={messages} />
    )
  }
}

const Component = ({ messages }) => (
  <div className="text-danger form-errors">
    <ul>
      {messages.map((entry, i) =>
        <li key={i}>
          <Markdown>{entry}</Markdown>
        </li>
      )}
    </ul>
  </div>
)

Component.propTypes = {
  messages: React.PropTypes.arrayOf(React.PropTypes.string).isRequired,
}

export default Container
